CREATE TABLE IF NOT EXISTS "__EFMigrationsHistory" (
    "MigrationId" character varying(150) NOT NULL,
    "ProductVersion" character varying(32) NOT NULL,
    CONSTRAINT "PK___EFMigrationsHistory" PRIMARY KEY ("MigrationId")
);

START TRANSACTION;
ALTER TABLE "Users" ADD "RefreshToken" text NOT NULL DEFAULT '';

ALTER TABLE "Users" ADD "RefreshTokenExpiryTime" timestamp with time zone NOT NULL DEFAULT TIMESTAMPTZ '-infinity';

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20250509162922_addTokenAttr', '9.0.4');

COMMIT;

