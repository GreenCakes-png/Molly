using Neo.SmartContract.Framework;
using Neo.SmartContract.Framework.Attributes;
using Neo.SmartContract.Framework.Interfaces;
using Neo.SmartContract.Framework.Native;
using Neo.SmartContract.Framework.Services;

using System;
using System.ComponentModel;
using System.Numerics;


namespace Neo.SmartContract.Template
{
    public partial class Molly : Nep11Token<PlayerTokenState>, INep24
    {
        private const byte Prefix_League = 0x0a;

        [Safe]
        public static bool DoesLeagueExist(ByteString League)
        {
            return Storage.Get((new[] { Prefix_League }).Concat(League)) != null;
        }

        [Safe]
        public static LeagueState GetLeague(ByteString League)
        {
            var league = Storage.Get((new[] { Prefix_League }).Concat(League));
            if(league == null) return null;
            return (LeagueState)StdLib.Deserialize(league);
        }

        public delegate void OnLeagueCreatedDelegate(ByteString League, BigInteger Start, BigInteger End);

        [DisplayName("LeagueCreated")]
        public static event OnLeagueCreatedDelegate OnLeagueCreated;

        public static void CreateLeague(ByteString League, BigInteger StartDate, BigInteger EndDate)
        {
            ExecutionEngine.Assert(IsOwner(), "No Authorization!");

            var leagueState = new LeagueState
            {
                Start = StartDate,
                End = EndDate
            };

            Storage.Put((new[] { Prefix_League }).Concat(League), StdLib.Serialize(leagueState));
            OnLeagueCreated(League, StartDate, EndDate);
        }
    }

    public class LeagueState
    {
        public BigInteger Start { get; set; }
        public BigInteger End { get; set; }
    }
}