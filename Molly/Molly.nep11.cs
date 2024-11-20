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
        private const byte PrefixMinter = 0xfd;

        private const byte PrefixCounter = 0xee;

        [Safe]
        public static UInt160 GetMinter()
        {
            var currentMinter = Storage.Get(new[] { PrefixMinter });

            if (currentMinter == null)
                return GetOwner();

            return (UInt160)currentMinter;
        }

        private static bool IsMinter() => Runtime.CheckWitness(GetMinter());

        public delegate void OnSetMinterDelegate(UInt160 newMinter);

        [DisplayName("SetMinter")]
        public static event OnSetMinterDelegate OnSetMinter;

        public static void SetMinter(UInt160? newMinter)
        {
            ExecutionEngine.Assert(IsOwner(), "No Authorization!");
            ExecutionEngine.Assert(newMinter != null && newMinter.IsValid && !newMinter.IsZero, "Wrong newMinter");

            Storage.Put(new[] { PrefixMinter }, newMinter);
            OnSetMinter(newMinter);
        }

        public static void MultiMint(UInt160 to, ByteString league, string data)
        {
            ExecutionEngine.Assert(DoesLeagueExist(league), "Unknown League");
            ExecutionEngine.Assert(IsOwner() || IsMinter(), "No Authorization!");
            List<List<string>> players = (List<List<string>>)StdLib.JsonDeserialize(data);

            for(var i = 0; i < players.Count; i++)
            {
                MyMint(to, players[i][0] + " " + players[i][1], players[i][0] + " " + players[i][1], players[i][2], league, players[i][3]);
            }
        }

        private static void MyMint(UInt160 to, string tokenId, string playerName, string playerPosition, ByteString league, ByteString img)
        {
            // IncreaseCount();
            // BigInteger counter = CurrentCount();
            var nep11Token = new PlayerTokenState()
            {
                Name = playerName,
                Owner = to,
                Position = playerPosition,
                Image = img,
                League = league
            };
            Mint(tokenId, nep11Token);

            //Transfer to coach
        }

        [Safe]
        public static BigInteger CurrentCount()
        {
            return (BigInteger)Storage.Get(new[] { PrefixCounter });
        }

        private static void IncreaseCount()
        {
            SetCount(CurrentCount() + 1);
        }
        
        private static void SetCount(BigInteger count)
        {
            Storage.Put(new[] { PrefixCounter }, count);
        }
    }
}