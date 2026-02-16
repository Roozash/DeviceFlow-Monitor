import sqlite3

class DatabaseManager:
    def __init__(self, db_path="device_flow_monitor.db"):
        self.db_path = db_path

    def get_connection(self):
        return sqlite3.connect(self.db_path)

    def init_db(self):
        with self.get_connection() as conn:
            cur = conn.cursor()
            print("DB initialized:", self.db_path)

            cur.execute("""
            CREATE TABLE IF NOT EXISTS metrics_queue (
                id INTEGER PRIMARY KEY AUTOINCREMENT,
                device_id TEXT NOT NULL,
                ts INTEGER NOT NULL,          -- unix ms
                created_at TEXT,              -- readable timestamp
                seq INTEGER NOT NULL,
                payload TEXT NOT NULL,        -- JSON string
                sent INTEGER NOT NULL DEFAULT 0,
                tries INTEGER NOT NULL DEFAULT 0,
                last_error TEXT
            );
            """)

            # Helps prevent duplicates (idempotency) even locally
            cur.execute("""
            CREATE UNIQUE INDEX IF NOT EXISTS uq_device_ts_seq
            ON metrics_queue(device_id, ts, seq);
            """)

            # Faster queries for unsent rows
            cur.execute("""
            CREATE INDEX IF NOT EXISTS ix_unsent
            ON metrics_queue(sent, ts);
            """)
            conn.commit()

    def save_metrics(self, device_id, ts, seq, payload):
        # Calculate readable time from unix timestamp (ms)
        from datetime import datetime
        dt = datetime.fromtimestamp(ts / 1000)
        created_at = dt.strftime("%Y-%m-%d %H:%M:%S")

        with self.get_connection() as conn:
            cur = conn.cursor()
            cur.execute("""
            INSERT INTO metrics_queue (device_id, ts, created_at, seq, payload)
            VALUES (?, ?, ?, ?, ?)
            """, (device_id, ts, created_at, seq, payload))
            conn.commit()
