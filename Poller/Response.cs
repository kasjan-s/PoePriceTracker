using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Poller
{
    /*
     * Everything below has been created with Edit -> Paste Special -> Paste JSON As Classes
     * So there's a lot of fields that aren't being currently used, but which will be probably useful in future
     * On the upside, Paste Special really speeds up the development process, as it eliminates human factor
     */

    public class Response
    {
        public int took { get; set; }
        public bool timed_out { get; set; }
        public _Shards _shards { get; set; }
        public Hits hits { get; set; }
    }

    public class _Shards
    {
        public int total { get; set; }
        public int successful { get; set; }
        public int failed { get; set; }
    }

    public class Hits
    {
        public int total { get; set; }
        public float max_score { get; set; }
        public Hit[] hits { get; set; }
    }

    public class Hit
    {
        public string _index { get; set; }
        public string _type { get; set; }
        public string _id { get; set; }
        public float _score { get; set; }
        public _Source _source { get; set; }
    }

    public class _Source
    {
        public Modspseudo modsPseudo { get; set; }
        public Info info { get; set; }
        public Requirements requirements { get; set; }
        public Modstotal modsTotal { get; set; }
        public string md5sum { get; set; }
        public string uuid { get; set; }
        public Properties properties { get; set; }
        public Shop shop { get; set; }
        public Sockets sockets { get; set; }
        public Mods mods { get; set; }
        public Attributes attributes { get; set; }
    }

    public class Modspseudo
    {
        public int eleResistSumLightning { get; set; }
        public int eleResistTotal { get; set; }
        public int eleResistSumFire { get; set; }
        public int eleResistSumCold { get; set; }
        public int maxLife { get; set; }
        public int flatSumInt { get; set; }
        public int flatSumDex { get; set; }
        public int flatAttributesTotal { get; set; }
    }

    public class Info
    {
        public string icon { get; set; }
        public string fullName { get; set; }
        public string[] flavourText { get; set; }
        public object descrText { get; set; }
        public string typeLine { get; set; }
        public string name { get; set; }
    }

    public class Requirements
    {
        public int Level { get; set; }
        public int Dex { get; set; }
        public int Int { get; set; }
    }

    public class Modstotal
    {
        public bool YourFireDamagecanShockbutnotIgnite { get; set; }
        public bool YourLightningDamagecanFreezebutnotShock { get; set; }
        public bool YourColdDamagecanIgnitebutnotFreezeorChill { get; set; }
        public int toallElementalResistances { get; set; }
        public int increasedGlobalCriticalStrikeChance { get; set; }
        public int reducedGlobalCriticalStrikeMultiplier { get; set; }
        public int increasedStunRecovery { get; set; }
        public int toDexterityandIntelligence { get; set; }
        public int tomaximumMana { get; set; }
        public int tomaximumLife { get; set; }
    }

    public class Properties
    {
        public Armour Armour { get; set; }
    }

    public class Armour
    {
        public int EnergyShield { get; set; }
        public int EvasionRating { get; set; }
    }

    public class Shop
    {
        public string lastUpdateDB { get; set; }
        public string sellerIGN { get; set; }
        public long added { get; set; }
        public int priceChanges { get; set; }
        public float chaosEquiv { get; set; }
        public long modified { get; set; }
        public string currency { get; set; }
        public float amount { get; set; }
        public string sellerAccount { get; set; }
        public string generatedWith { get; set; }
        public string forumID { get; set; }
        public string threadid { get; set; }
        public long updated { get; set; }
        public string verified { get; set; }
    }

    public class Sockets
    {
        public string allSocketsSorted { get; set; }
        public Sortedlinkgroup sortedLinkGroup { get; set; }
        public int socketCount { get; set; }
        public string allSockets { get; set; }
        public int largestLinkGroup { get; set; }
    }

    public class Sortedlinkgroup
    {
        public string _1 { get; set; }
        public string _0 { get; set; }
    }

    public class Mods
    {
        public Helmet Helmet { get; set; }
        public Amulet Amulet { get; set; }
    }

    public class Helmet
    {
        public Explicit _explicit { get; set; }
    }

    public class Explicit
    {
        public bool YourFireDamagecanShockbutnotIgnite { get; set; }
        public bool YourLightningDamagecanFreezebutnotShock { get; set; }
        public bool YourColdDamagecanIgnitebutnotFreezeorChill { get; set; }
        public int toallElementalResistances { get; set; }
    }

    public class Amulet
    {
        public Explicit1 _explicit { get; set; }
        public Implicit _implicit { get; set; }
    }

    public class Explicit1
    {
        public int increasedGlobalCriticalStrikeChance { get; set; }
        public int reducedGlobalCriticalStrikeMultiplier { get; set; }
        public int increasedStunRecovery { get; set; }
        public int tomaximumMana { get; set; }
        public int tomaximumLife { get; set; }
    }

    public class Implicit
    {
        public int toDexterityandIntelligence { get; set; }
    }

    public class Attributes
    {
        public string equipType { get; set; }
        public bool support { get; set; }
        public int implicitModCount { get; set; }
        public int cosmeticModCount { get; set; }
        public bool corrupted { get; set; }
        public int craftedModCount { get; set; }
        public bool lockedToCharacter { get; set; }
        public bool identified { get; set; }
        public string baseItemType { get; set; }
        public int inventoryHeight { get; set; }
        public int explicitModCount { get; set; }
        public string rarity { get; set; }
        public bool mirrored { get; set; }
        public int frameType { get; set; }
        public string league { get; set; }
        public string itemType { get; set; }
        public int inventoryWidth { get; set; }
    }
}
