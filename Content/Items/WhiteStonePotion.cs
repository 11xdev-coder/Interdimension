using Terraria.ID;
using Terraria.ModLoader;
using Terraria;
using Interdimension.Content.Buffs;
using Terraria.Audio;

namespace Interdimension.Content.Items;

public class WhiteStonePotion : ModItem 
{
    public override void SetDefaults()
    {
        Item.width = 20;
        Item.height = 26;
        Item.useStyle = ItemUseStyleID.DrinkOld;
        Item.UseSound = SoundID.Item3;
        Item.useAnimation = 15;
        Item.useTime = 15;
        Item.useTurn = true;
        Item.consumable = true;
        Item.maxStack = Item.CommonMaxStack;
        Item.rare = ItemRarityID.Cyan;
        Item.value = Item.buyPrice(gold: 1);
        Item.buffType = ModContent.BuffType<WhiteStoneBuff>();
        Item.buffTime = 60 * 60;
    }

    public override bool? UseItem(Player player)
    {
        SpawnDust(player);
        Explode(player);
        return true;
    }
    
    private void SpawnDust(Player player) 
    {
        for (int i = 0; i < 1000; i++) 
        {
            Dust.NewDust(player.position, player.width, player.height, DustID.MagicMirror, 
            Main.rand.NextFloat(-1f, 1f), Main.rand.NextFloat(-1f, 1f));
        }
    }
    
    private void Explode(Player player) 
    {
        int radius = 15;
        SoundEngine.PlaySound(SoundID.Item14, player.Center);
        if (player.whoAmI != Main.myPlayer) return;
        
        int centerX = (int)(player.Center.X / 16f);
        int centerY = (int)(player.Center.Y / 16f);
        
        for (int x = centerX - radius; x < centerX + radius; x++) 
        {
            for (int y = centerY - radius; y < centerY + radius; y++)
            {
                if (!WorldGen.InWorld(x, y))
                    continue;
                    
                int dx = x - centerX;
                int dy = y - centerY;
                if (dx * dx + dy * dy > radius * radius)
                    continue;
                    
                Tile tile = Main.tile[x, y];
                if (!tile.HasTile) continue;
                
                WorldGen.KillTile(x, y);
                if (Main.netMode == NetmodeID.MultiplayerClient)
                {
                    NetMessage.SendData(MessageID.TileManipulation, -1, -1, null, 0, x, y);
                }
            }
        }
    }
}