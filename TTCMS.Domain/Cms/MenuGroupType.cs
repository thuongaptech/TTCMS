using System.ComponentModel;

namespace TTCMS.Domain
{
    public enum MenuGroupType
    {
        [Description("Main Menu")]
        MainMenu,
        [Description("Top Menu")]
        TopMenu,
        [Description("Bot Menu")]
        BotMenu,
        [Description("Category Menu")]
        Category,
        [Description("Category Menu Mobile")]
        Category_Mobile,
        [Description("Page Menu")]
        Page_Menu,
    }
}
