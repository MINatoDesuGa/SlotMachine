public static class GameConfig
{
    public const int MAX_SLOTS = 3;

    public const float SLOT_SPIN_DELAY = 0.025f; //delay between each spin transform updates 
    public const float SLOT_SPIN_MAX_yTHRESHOLD = 1.5f; //max yPos to move before resetting to first dummy symbol pos
    public const float SLOT_SPIN_RESET_yPOS = -4.5f; //first dummy symbol yPos to reset to (mimic circular motion of symbols in 2d)
    public const float SLOT_SPIN_SPEED = 0.25f; //move distance each update

    public const int MAX_SYMBOLS = 4; 
    public const int MAX_FINAL_SPIN_ROUNDS = 3; //after random symbol pos calculation, max rounds to visit calculated symbol pos before coming to halt 

    public const float BAR_SYMBOL_yPOS = 0.75f; //Bar symbol yPos in slot holder
    public const float JACKPOT_SYMBOL_yPOS = -0.75f; //jackpot(7) symbol yPos in slot holder
    public const float BERRIES_SYMBOL_yPOS = -2.25f; //berries symbol yPos in slot holder
    public const float BELL_SYMBOL_yPOS = -3.75f; //bell symbol yPos in slot holder

    public static readonly float[] SLOT_SYMBOLS_yPOS = { //symbols yPos array (index will be used as parameter for random number generation)
        BAR_SYMBOL_yPOS,
        JACKPOT_SYMBOL_yPOS,
        BERRIES_SYMBOL_yPOS,
        BELL_SYMBOL_yPOS
    };
}
//------ Global refs ------//
public enum SlotSymbols {
    BAR, JACKPOT, BERRIES, BELL
}
public enum SymbolMatchStatus {
    NONE, DOUBLE, TRIPLE, JACKPOT
}