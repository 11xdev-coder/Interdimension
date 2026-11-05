using Terraria;
using Terraria.ModLoader;

namespace Interdimension.Content.Buffs;

public class WhiteStoneBuff : ModBuff 
{
    public override void Update(Player player, ref int buffIndex)
    {
        player.statDefense += 15;
    }
}