CREATE TABLE [Termini].[Termin.Players] (
    [Id]           INT      IDENTITY (1, 1) NOT NULL,
    [Active]       BIT      CONSTRAINT [DF_Termini_Termin_Players_Active] DEFAULT ((1)) NOT NULL,
    [DateCreated]  DATETIME CONSTRAINT [DF_Termini_Termin_Players_DateCreated] DEFAULT (getdate()) NOT NULL,
    [TerminId]     INT      NOT NULL,
    [PlayerId]     INT      NOT NULL,
    [PlayerRating] INT      NULL,
    CONSTRAINT [PK_Termini_Termin_Players_Id] PRIMARY KEY CLUSTERED ([Id] ASC),
    CONSTRAINT [FK_Termini_Termin_Players_PlayerId] FOREIGN KEY ([PlayerId]) REFERENCES [Termini].[Player] ([Id]),
    CONSTRAINT [FK_Termini_Termin_Players_TerminId] FOREIGN KEY ([TerminId]) REFERENCES [Termini].[Termin] ([Id])
);




GO
CREATE NONCLUSTERED INDEX [IX_Termini_Termin_Termin_with_player]
    ON [Termini].[Termin.Players]([TerminId] ASC, [PlayerId] ASC) WHERE ([Active]=(1));


GO
CREATE NONCLUSTERED INDEX [IX_Termini_Termin_PlayerId]
    ON [Termini].[Termin.Players]([PlayerId] ASC) WHERE ([Active]=(1));


GO
CREATE NONCLUSTERED INDEX [IX_Termini_Termin_TerminId]
    ON [Termini].[Termin.Players]([TerminId] ASC) WHERE ([Active]=(1));

