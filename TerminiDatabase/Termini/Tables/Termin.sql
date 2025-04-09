CREATE TABLE [Termini].[Termin] (
    [Id]              INT      IDENTITY (1, 1) NOT NULL,
    [Active]          BIT      CONSTRAINT [DF_Termini_Termin_Active] DEFAULT ((1)) NOT NULL,
    [DateCreated]     DATETIME CONSTRAINT [DF_Termini_Termin_DateCreated] DEFAULT (getdate()) NOT NULL,
    [ScheduledDate]   DATE     NOT NULL,
    [StartTime]       TIME (7) NOT NULL,
    [DurationMinutes] INT      NOT NULL,
    CONSTRAINT [PK_Termini_Termin_Id] PRIMARY KEY CLUSTERED ([Id] ASC)
);


GO
CREATE NONCLUSTERED INDEX [IX_Termini_Termin_ScheduledDate_StartTime]
    ON [Termini].[Termin]([ScheduledDate] ASC, [StartTime] ASC) WHERE ([Active]=(1));

