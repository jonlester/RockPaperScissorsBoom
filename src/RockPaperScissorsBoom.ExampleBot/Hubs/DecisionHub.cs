using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using RockPaperScissor.Core.Game;
using RockPaperScissor.Core.Game.Bots;
using RockPaperScissor.Core.Game.Results;

namespace RockPaperScissorsBoom.ExampleBot.Hubs
{
    public class DecisionHub : Hub
    {
        static Guid _matchId = Guid.Empty;
        static int _round = 0;
        public async Task RequestMove(PreviousDecisionResult previousDecision)
        {
            BaseBot bot;
            if (previousDecision.MatchId != _matchId)
            {
                _matchId = previousDecision.MatchId;
                _round = 1;
            }

            if(_round < 100 && previousDecision.OpponentPrevious != Decision.WaterBalloon)
            {
                bot = new DynamiteOnlyBot(); 
            }
            else
            {
                bot = new CleverBot();
            }
            _round++;

            Decision decision = bot.GetDecision(previousDecision);
            await Clients.All.SendAsync("MakeDecision", decision);
        }
    }
}