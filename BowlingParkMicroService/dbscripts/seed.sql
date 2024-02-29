\connect bowlingpark-bdd

-- Table: public.bowlingPark

-- DROP TABLE IF EXISTS public."bowlingPark";

CREATE TABLE IF NOT EXISTS public."bowlingPark"
(
    "Id" text COLLATE pg_catalog."default" NOT NULL,
    "Adress" text COLLATE pg_catalog."default" NOT NULL,
    "ManagerId" integer NOT NULL,
    CONSTRAINT "PK_bowlingPark" PRIMARY KEY ("Id")
    )

    TABLESPACE pg_default;

ALTER TABLE IF EXISTS public."bowlingPark"
    OWNER to admin;

-- Table: public.bowlingAlleys

-- DROP TABLE IF EXISTS public."bowlingAlleys";

CREATE TABLE IF NOT EXISTS public."bowlingAlleys"
(
    "AlleyNumber" integer NOT NULL,
    "BowlingParkId" text COLLATE pg_catalog."default" NOT NULL,
    "QrCode" text COLLATE pg_catalog."default" NOT NULL,
    CONSTRAINT "PK_bowlingAlleys" PRIMARY KEY ("BowlingParkId", "AlleyNumber"),
    CONSTRAINT "FK_bowlingAlleys_bowlingPark_BowlingParkId" FOREIGN KEY ("BowlingParkId")
    REFERENCES public."bowlingPark" ("Id") MATCH SIMPLE
    ON UPDATE NO ACTION
    ON DELETE CASCADE
    )

    TABLESPACE pg_default;

ALTER TABLE IF EXISTS public."bowlingAlleys"
    OWNER to admin;