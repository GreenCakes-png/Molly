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
        private const byte Prefix_Owner = 0xff;

        [Safe]
        public static UInt160 GetOwner() => (UInt160)Storage.Get(new[] { Prefix_Owner });

        private static bool IsOwner() => Runtime.CheckWitness(GetOwner());

        public delegate void OnSetOwnerDelegate(UInt160 newOwner);

        [DisplayName("SetOwner")]
        public static event OnSetOwnerDelegate OnSetOwner;

        public static void SetOwner(UInt160? newOwner)
        {
            ExecutionEngine.Assert(IsOwner(), "No Authorization!");
            ExecutionEngine.Assert(newOwner != null && newOwner.IsValid && !newOwner.IsZero, "Wrong newOwner");

            Storage.Put(new[] { Prefix_Owner }, newOwner);
            OnSetOwner(newOwner);
        }
    }
}