using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class DisasterGenerator : SingletonBehaviour<DisasterGenerator> {

    List<Disaster> disasters;

    void Start() {
        InitDisasters();
    }

    void InitDisasters() {
        disasters = new List<Disaster>
        {
            new Disaster(id: 0, text: "Crew Mutiny!", requiredWine: 10, requiredFood: 7, requiredFleece: 0),//17
            new Disaster(id: 1, text: "Poseidon's Wrath!", requiredWine: 0, requiredFood: 11, requiredFleece: 4),//15
            new Disaster(id: 2, text: "Cold Winter!", requiredWine: 3, requiredFood: 5, requiredFleece: 8),//16
            new Disaster(id: 3, text: "Released Winds!", requiredWine: 6, requiredFood: 3, requiredFleece: 5),//14
            new Disaster(id: 4, text: "Song of the Sirens!", requiredWine: 8, requiredFood: 0, requiredFleece: 4),//12
            new Disaster(id: 5, text: "Storm!", requiredWine: 0, requiredFood: 7, requiredFleece: 6),//13
            new Disaster(id: 6, text: "Demands from Dionysis!", requiredWine: 13, requiredFood: 3, requiredFleece: 0),//16
            new Disaster(id: 7, text: "Demands from Artemis!", requiredWine: 0, requiredFood: 4, requiredFleece: 13)//17
        };
    }

    public Disaster GetRandomDisaster() {
        return disasters[Random.Range(0, disasters.Count - 1)];
    }
}
