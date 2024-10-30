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
    public partial class Molly
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

        public static void Mint(UInt160 to, string playerName, string playerPosition, string league)
        {
            ExecutionEngine.Assert(DoesLeagueExist(league), "Unknown League");
            ExecutionEngine.Assert(IsOwner() || IsMinter(), "No Authorization!");
            IncreaseCount();
            BigInteger tokenId = CurrentCount();
            var nep11Token = new PlayerTokenState()
            {
                Name = playerName,
                Owner = to,
                Position = playerPosition,
                League = league
            };
            Mint((ByteString)tokenId + " " + playerName, nep11Token);
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