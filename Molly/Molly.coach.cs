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
        private const byte Prefix_Coach = 0xab;

        [Safe]
        public static UInt160 GetCoach() => (UInt160)Storage.Get(new[] { Prefix_Coach });

        public delegate void OnSetCoachDelegate(UInt160 newCoach);

        [DisplayName("SetCoach")]
        public static event OnSetCoachDelegate OnSetCoach;

        public static void SetCoach(UInt160? newCoach)
        {
            ExecutionEngine.Assert(IsOwner(), "No Authorization!");

            Storage.Put(new[] { Prefix_Coach }, newCoach);
            OnSetCoach(newCoach);
        }
    }
}
