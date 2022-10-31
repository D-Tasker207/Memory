using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MemoryGame : MonoBehaviour
{
    // Suit and Rank names must match Free_Playing_Cards convention
    string[] kCardsSuits = new string[] {"Club", "Diamond", "Spades", "Heart" };
    string[] kCardsRanks = new string[] {"2", "3", "4", "5", "6", "7", "8", "9", "10", "J", "Q", "K", "A" };
    
    static public MemoryGame instance;

    private Card[] cards;
    private Card selectOne;
    private Card selectTwo;
    private double selectTime;

    // Start is called before the first frame update
    void Start()
    {
        instance = this;

        //Get All Cards on gameboard
        cards = transform.GetComponentsInChildren<Card>();

        // Deal random cards in pairs
        int n = 0;
        Shuffle(cards);
        for (int i = 0; i < cards.Length/2; i++)
        {
            //choose random suit and rank
            string suit = GetRandomFromArray(kCardsSuits);
            string rank = GetRandomFromArray(kCardsRanks);
            //assign it to two cards
            cards[n++].SetSuitandRank(suit, rank);
            cards[n++].SetSuitandRank(suit, rank);
        }
    }

    private void Shuffle<T> (T[] array)
    {
        int n = array.Length;
        while (n>1)
        {
            int k = (int)Mathf.Floor(Random.value * (n--));
            T temp = array[n];
            array[n] = array[k];
            array[k] = temp;
        }
    }

    private T GetRandomFromArray<T> (T[] array)
    {
        return array[(int)Mathf.Floor(Random.value * (array.Length))];
    }

    public void Select(Card card)
    {
        //if we don't have two selected cards
        if (selectTwo == null) {
            //flip card
            card.Flip();
            //save card in selectOne or selectTwo
            if (selectOne == null) {
                selectOne = card;
            } else {
                selectTwo = card;
                selectTime = Time.time;
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        //check for match or misatch
        if (selectTwo != null) {
            //wait one second so user can see cards
            if (Time.time - selectTime > 1) {
                CheckMatch();
            }
        }
    }

    private void CheckMatch(){
        if (selectOne.Matches(selectTwo)) {
            //remove cards from board
            selectOne.Hide();
            selectTwo.Hide();
        } else {
            //return cards face down
            selectOne.Flip();
            selectTwo.Flip();
        }

        //clear selection
        selectOne = selectTwo = null;   
    }
}
