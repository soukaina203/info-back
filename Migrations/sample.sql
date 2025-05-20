CREATE TABLE IF NOT EXISTS "__EFMigrationsHistory" (
    "MigrationId" character varying(150) NOT NULL,
    "ProductVersion" character varying(32) NOT NULL,
    CONSTRAINT "PK___EFMigrationsHistory" PRIMARY KEY ("MigrationId")
);

START TRANSACTION;
ALTER TABLE "Users" ALTER COLUMN "Status" TYPE integer;

ALTER TABLE "Users" ALTER COLUMN "IsAdmin" DROP NOT NULL;

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20250520141655_changeStatusColumn', '9.0.4');

ALTER TABLE "Users" ALTER COLUMN "Status" TYPE text;

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20250520142123_changeStatusColumn1', '9.0.4');

COMMIT;

