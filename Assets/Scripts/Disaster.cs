using System.Collections.Generic;

public class Disaster {
    public int id;
    public string text;
    public int requiredWine;
    public int requiredFood;
    public int requiredFleece;

    public Disaster(int id, string text, int requiredWine, int requiredFood, int requiredFleece) {
        this.id = id;
        this.text = text;
        this.requiredWine = requiredWine;
        this.requiredFood = requiredFood;
        this.requiredFleece = requiredFleece;
    }
}
