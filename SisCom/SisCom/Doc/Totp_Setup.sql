-- TOTP (Microsoft/Google Authenticator) setup script
-- Chay tay script nay tren DB truoc khi deploy tinh nang TOTP.

CREATE TABLE sys_totp_setting (
  id INT IDENTITY PRIMARY KEY,
  is_enabled BIT NOT NULL DEFAULT 0,
  updated_by VARCHAR(50) NULL,
  updated_date DATETIME NULL
);
INSERT INTO sys_totp_setting(is_enabled) VALUES (0); -- luon chi 1 dong

CREATE TABLE user_totp (
  user_id INT NOT NULL PRIMARY KEY,
  secret_key VARCHAR(100) NOT NULL,
  is_confirmed BIT NOT NULL DEFAULT 0,
  created_date DATETIME NOT NULL DEFAULT GETDATE(),
  confirmed_date DATETIME NULL
);
