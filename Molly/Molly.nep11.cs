using Neo.SmartContract.Framework;
using Neo.SmartContract.Framework.Attributes;
using Neo.SmartContract.Framework.Interfaces;
using Neo.SmartContract.Framework.Native;
using Neo.SmartContract.Framework.Services;

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

        public static BigInteger MyMint(string tokenId, string playerName, string playerTeam, string playerPosition, ByteString league, ByteString img)
        {
            ExecutionEngine.Assert(DoesLeagueExist(league), "Unknown League");
            ExecutionEngine.Assert(Runtime.CallingScriptHash == GetCoach(), "No Authorization!");

            IncreaseCount();
            BigInteger counter = CurrentCount();

            var nep11Token = new PlayerTokenState()
            {
                Name = playerName,
                Owner = Runtime.CallingScriptHash,
                Position = playerPosition,
                Image = img,
                League = league,
                Team = playerTeam
            };
            Mint(counter.ToString(), nep11Token);
            return counter;
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