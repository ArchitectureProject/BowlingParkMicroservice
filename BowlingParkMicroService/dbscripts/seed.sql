\connect bowlingpark-bdd

-- Table: public.bowlingPark

-- DROP TABLE IF EXISTS public."bowlingPark";

CREATE TABLE IF NOT EXISTS public."bowlingPark"
(
    "Id" text COLLATE pg_catalog."default" NOT NULL,
    "Adress" text COLLATE pg_catalog."default" NOT NULL,
    "ManagerId" text NOT NULL,
    CONSTRAINT "PK_bowlingPark" PRIMARY KEY ("Id")
    )

    TABLESPACE pg_default;

ALTER TABLE IF EXISTS public."bowlingPark"
    OWNER to admin;

-- Table: public.bowlingAlley

-- DROP TABLE IF EXISTS public."bowlingAlley";

CREATE TABLE IF NOT EXISTS public."bowlingAlley"
(
    "AlleyNumber" integer NOT NULL,
    "BowlingParkId" text COLLATE pg_catalog."default" NOT NULL,
    "QrCode" text COLLATE pg_catalog."default" NOT NULL,
    CONSTRAINT "PK_bowlingAlley" PRIMARY KEY ("BowlingParkId", "AlleyNumber"),
    CONSTRAINT "FK_bowlingAlley_bowlingPark_BowlingParkId" FOREIGN KEY ("BowlingParkId")
    REFERENCES public."bowlingPark" ("Id") MATCH SIMPLE
    ON UPDATE NO ACTION
    ON DELETE CASCADE
    )

    TABLESPACE pg_default;

ALTER TABLE IF EXISTS public."bowlingAlley"
    OWNER to admin;