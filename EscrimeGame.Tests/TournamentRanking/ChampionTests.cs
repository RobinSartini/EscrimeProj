using EscrimeGame;
using FluentAssertions;
using Xunit;
using System.Collections.Generic;
using static EscrimeGame.Tests.MatchResults;

namespace EscrimeGame.Tests;

public class ChampionTests
{
    private readonly ScoreCalculator _calculator = new();
    private readonly TournamentRanking _ranking;

    public ChampionTests()
    {
        _ranking = new TournamentRanking(_calculator);
    }

    [Fact]
    [Trait("Requirement", "REQ-E-014")]
    [Trait("TestCase", "TC-022")]
    public void GetChampion_AllPlayersDisqualified_ChampionScoreIsZero()
    {
        var players = new List<Player>
        {
            new Player { Name = "Sir Galahad", Matches = new List<MatchResult> { Win, Win, Win }, IsDisqualified = true },
            new Player { Name = "Lancelot",   Matches = new List<MatchResult> { Win, Win }, IsDisqualified = true },
            new Player { Name = "Dame Morgane", Matches = new List<MatchResult> { Win }, IsDisqualified = true },
        };

        var champion = _ranking.GetChampion(players);

        champion.Should().NotBeNull();

        var championScore = _calculator.CalculateScore(
            champion!.Matches,
            isDisqualified: champion.IsDisqualified,
            penaltyPoints: champion.PenaltyPoints);

        championScore.Should().Be(0, "tous disqualifiés → meilleur score possible = 0");
    }
}
