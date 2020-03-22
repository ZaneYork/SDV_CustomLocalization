using System.Collections.Generic;
using System.Reflection;
using Harmony;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using StardewValley;
using StardewValley.BellsAndWhistles;
using StardewValley.Menus;
using StardewValley.Mobile;
using StardewValley.Objects;
using StardewValley.Projectiles;

namespace StardewModdingAPI.Mods.CustomLocalization
{
    public class ModEntry : Mod
    {
        public static ModConfig ModConfig;

        public static IMonitor monitor;

        public static string ModPath;

        private static ModEntry Instance;

        public static void SaveConfig()
        {
            Instance.Helper.WriteConfig(ModConfig);
        }

        public override void Entry(IModHelper helper)
        {
            Instance = this;
            ModConfig = helper.ReadConfig<ModConfig>();
            ModPath = helper.DirectoryPath;
            monitor = this.Monitor;
            HarmonyInstance harmony = HarmonyInstance.Create("zaneyork.CustomLocalization");
            harmony.PatchAll(Assembly.GetExecutingAssembly());

            Dictionary<string, bool> dictionary = this.Helper.Reflection.GetField<Dictionary<string, bool>>(Game1.content, "_localizedAsset").GetValue();
            Dictionary<string, object> dictionaryAssets = this.Helper.Reflection.GetField<Dictionary<string, object>>(Game1.content, "loadedAssets").GetValue();
            dictionary.Clear();
            dictionaryAssets.Clear();
            LoadContent();
            if (ModConfig.CurrentLanguageCode > ModConfig.OriginLocaleCount)
            {
                monitor.Log($"Restore locale to : {ModConfig.CurrentLanguageCode}");
                LocalizedContentManager.CurrentLanguageCode = (LocalizedContentManager.LanguageCode)ModConfig.CurrentLanguageCode;
            }
            else
            {
                if (!LocalizedContentManager.CurrentLanguageLatin)
                {
                    this.Helper.Reflection.GetMethod(typeof(SpriteText), "OnLanguageChange").Invoke(LocalizedContentManager.CurrentLanguageCode);
                }
            }
        }

        private void LoadContent()
        {
            Game1.concessionsSpriteSheet = ((ContentManager)Game1.content).Load<Texture2D>("LooseSprites\\Concessions");
            Game1.birdsSpriteSheet = ((ContentManager)Game1.content).Load<Texture2D>("LooseSprites\\birds");
            Game1.daybg = ((ContentManager)Game1.content).Load<Texture2D>("LooseSprites\\daybg");
            Game1.nightbg = ((ContentManager)Game1.content).Load<Texture2D>("LooseSprites\\nightbg");
            Game1.menuTexture = ((ContentManager)Game1.content).Load<Texture2D>("Maps\\MenuTiles");
            Game1.uncoloredMenuTexture = ((ContentManager)Game1.content).Load<Texture2D>("Maps\\MenuTilesUncolored");
            Game1.lantern = ((ContentManager)Game1.content).Load<Texture2D>("LooseSprites\\Lighting\\lantern");
            Game1.windowLight = ((ContentManager)Game1.content).Load<Texture2D>("LooseSprites\\Lighting\\windowLight");
            Game1.sconceLight = ((ContentManager)Game1.content).Load<Texture2D>("LooseSprites\\Lighting\\sconceLight");
            Game1.cauldronLight = ((ContentManager)Game1.content).Load<Texture2D>("LooseSprites\\Lighting\\greenLight");
            Game1.indoorWindowLight = ((ContentManager)Game1.content).Load<Texture2D>("LooseSprites\\Lighting\\indoorWindowLight");
            Game1.shadowTexture = ((ContentManager)Game1.content).Load<Texture2D>("LooseSprites\\shadow");
            Game1.mouseCursors = ((ContentManager)Game1.content).Load<Texture2D>("LooseSprites\\Cursors");
            Game1.mouseCursors2 = ((ContentManager)Game1.content).Load<Texture2D>("LooseSprites\\Cursors2");
            Game1.giftboxTexture = ((ContentManager)Game1.content).Load<Texture2D>("LooseSprites\\Giftbox");
            Game1.controllerMaps = ((ContentManager)Game1.content).Load<Texture2D>("LooseSprites\\ControllerMaps");
            Game1.animations = ((ContentManager)Game1.content).Load<Texture2D>("TileSheets\\animations");
            Game1.mobileSpriteSheet = ((ContentManager)Game1.content).Load<Texture2D>("LooseSprites\\MobileAtlas_manually_made");
            Game1.achievements = (Dictionary<int, string>)((ContentManager)Game1.content).Load<Dictionary<int, string>>("Data\\Achievements");
            Game1.NPCGiftTastes = (IDictionary<string, string>)((ContentManager)Game1.content).Load<Dictionary<string, string>>("Data\\NPCGiftTastes");
            Game1.onScreenMenus.Clear();
            this.Helper.Reflection.GetMethod(Game1.game1, "TranslateFields").Invoke();
            Game1.dayTimeMoneyBox = new DayTimeMoneyBox();
            Game1.dayTimeMoneyBox.game1 = Game1.game1;
            Game1.onScreenMenus.Add((IClickableMenu)Game1.dayTimeMoneyBox);
            Game1.toolbar = new Toolbar();
            Game1.virtualJoypad = new VirtualJoypad();
            Game1.onScreenMenus.Add((IClickableMenu)Game1.toolbar);
            Game1.buffsDisplay = new BuffsDisplay();
            Game1.onScreenMenus.Add((IClickableMenu)Game1.buffsDisplay);
            Game1.dialogueFont = (SpriteFont)((ContentManager)Game1.content).Load<SpriteFont>("Fonts\\SpriteFont1");
            Game1.smallFont = (SpriteFont)((ContentManager)Game1.content).Load<SpriteFont>("Fonts\\SmallFont");
            Game1.tinyFont = (SpriteFont)((ContentManager)Game1.content).Load<SpriteFont>("Fonts\\tinyFont");
            Game1.tinyFontBorder = (SpriteFont)((ContentManager)Game1.content).Load<SpriteFont>("Fonts\\tinyFontBorder");
            Game1.objectSpriteSheet = ((ContentManager)Game1.content).Load<Texture2D>("Maps\\springobjects");
            Game1.cropSpriteSheet = ((ContentManager)Game1.content).Load<Texture2D>("TileSheets\\crops");
            Game1.emoteSpriteSheet = ((ContentManager)Game1.content).Load<Texture2D>("TileSheets\\emotes");
            Game1.debrisSpriteSheet = ((ContentManager)Game1.content).Load<Texture2D>("TileSheets\\debris");
            Game1.bigCraftableSpriteSheet = ((ContentManager)Game1.content).Load<Texture2D>("TileSheets\\Craftables");
            Game1.rainTexture = ((ContentManager)Game1.content).Load<Texture2D>("TileSheets\\rain");
            Game1.buffsIcons = ((ContentManager)Game1.content).Load<Texture2D>("TileSheets\\BuffsIcons");
            Game1.objectInformation = (IDictionary<int, string>)((ContentManager)Game1.content).Load<Dictionary<int, string>>("Data\\ObjectInformation");
            Game1.clothingInformation = (IDictionary<int, string>)((ContentManager)Game1.content).Load<Dictionary<int, string>>("Data\\ClothingInformation");
            Game1.objectContextTags = (IDictionary<string, string>)((ContentManager)Game1.content).Load<Dictionary<string, string>>("Data\\ObjectContextTags");
            Game1.bigCraftablesInformation = (IDictionary<int, string>)((ContentManager)Game1.content).Load<Dictionary<int, string>>("Data\\BigCraftablesInformation");
            FarmerRenderer.hairStylesTexture = ((ContentManager)Game1.content).Load<Texture2D>("Characters\\Farmer\\hairstyles");
            FarmerRenderer.shirtsTexture = ((ContentManager)Game1.content).Load<Texture2D>("Characters\\Farmer\\shirts");
            FarmerRenderer.pantsTexture = ((ContentManager)Game1.content).Load<Texture2D>("Characters\\Farmer\\pants");
            FarmerRenderer.hatsTexture = ((ContentManager)Game1.content).Load<Texture2D>("Characters\\Farmer\\hats");
            FarmerRenderer.accessoriesTexture = ((ContentManager)Game1.content).Load<Texture2D>("Characters\\Farmer\\accessories");
            Furniture.furnitureTexture = ((ContentManager)Game1.content).Load<Texture2D>("TileSheets\\furniture");
            SpriteText.spriteTexture = ((ContentManager)Game1.content).Load<Texture2D>("LooseSprites\\font_bold");
            SpriteText.coloredTexture = ((ContentManager)Game1.content).Load<Texture2D>("LooseSprites\\font_colored");
            Tool.weaponsTexture = ((ContentManager)Game1.content).Load<Texture2D>("TileSheets\\weapons");
            Projectile.projectileSheet = ((ContentManager)Game1.content).Load<Texture2D>("TileSheets\\Projectiles");
        }
    }
}
