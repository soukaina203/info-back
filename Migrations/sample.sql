CREATE TABLE IF NOT EXISTS "__EFMigrationsHistory" (
    "MigrationId" character varying(150) NOT NULL,
    "ProductVersion" character varying(32) NOT NULL,
    CONSTRAINT "PK___EFMigrationsHistory" PRIMARY KEY ("MigrationId")
);

START TRANSACTION;
ALTER TABLE "Methods" DROP CONSTRAINT "FK_Methods_ProfProfiles_ProfProfileId";

ALTER TABLE "Niveaux" DROP CONSTRAINT "FK_Niveaux_ProfProfiles_ProfProfileId";

ALTER TABLE "Services" DROP CONSTRAINT "FK_Services_ProfProfiles_ProfProfileId";

ALTER TABLE "Specialities" DROP CONSTRAINT "FK_Specialities_ProfProfiles_ProfProfileId";

DROP INDEX "IX_Specialities_ProfProfileId";

DROP INDEX "IX_Services_ProfProfileId";

DROP INDEX "IX_Niveaux_ProfProfileId";

DROP INDEX "IX_Methods_ProfProfileId";

ALTER TABLE "Specialities" DROP COLUMN "ProfProfileId";

ALTER TABLE "Services" DROP COLUMN "ProfProfileId";

ALTER TABLE "Niveaux" DROP COLUMN "ProfProfileId";

ALTER TABLE "Methods" DROP COLUMN "ProfProfileId";

ALTER TABLE "Services" RENAME COLUMN image TO "Image";

ALTER TABLE "ProfProfiles" ADD "Methodes" integer[] NOT NULL DEFAULT ARRAY[]::integer[];

ALTER TABLE "ProfProfiles" ADD "Niveaux" integer[] NOT NULL DEFAULT ARRAY[]::integer[];

ALTER TABLE "ProfProfiles" ADD "Services" integer[] NOT NULL DEFAULT ARRAY[]::integer[];

ALTER TABLE "ProfProfiles" ADD "Specialities" integer[] NOT NULL DEFAULT ARRAY[]::integer[];

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20250505200953_changeProfProfile', '9.0.4');

COMMIT;

