using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Threading;


int rep = 50000;
int handCount = 0;
HandType h = HandType.High;
int money = 1000;
int currentBet = 0;


for (int i = 0; i < rep; i++)
{
    Hand hand;
    List<Card> icards = new List<Card>();
    Deck deck = new Deck( i /*Convert.ToInt32(Console.ReadLine())*/);
    Console.Clear();
    deck.CreateDeck();
    icards.Add(deck.Draw());
    icards.Add(deck.Draw());
    icards.Add(deck.Draw());
    icards.Add(deck.Draw());
    icards.Add(deck.Draw());

    hand = new Hand(icards);

    icards.Clear();

    icards.Add(deck.Draw());
    icards.Add(deck.Draw());
    icards.Add(deck.Draw());
    icards.Add(deck.Draw());
    icards.Add(deck.Draw());

    Hand compHand = new Hand(icards);

    int comp = hand.Compare(compHand);

    hand.WriteCards();

    Console.WriteLine();
    Console.WriteLine("---");
    Console.WriteLine();

    Console.WriteLine("Type : " + hand.CheckCards().HandType);

    Console.WriteLine();
    Console.WriteLine("---");
    Console.WriteLine();

    compHand.WriteCards();

    Console.WriteLine();
    Console.WriteLine("---");
    Console.WriteLine();

    Console.WriteLine("Type : " + compHand.CheckCards().HandType);

    Console.WriteLine();
    Console.WriteLine("---");
    Console.WriteLine();

    Console.WriteLine("Winning Hand : " + comp.ToString());

    

    if(hand.CheckCards().HandType==h || compHand.CheckCards().HandType == h )
    {
        int reservedint = 0;
    }

    if (hand.CheckCards().HandType == h )
    {
        handCount++;
    }

    Console.Clear();
}

enum HandType {High,Pair,TwoPair,Three,Straight,Flush,FullHouse,Four,StraightFlush }

 class HandValue
{
    public HandValue()
    {
        HandType = HandType.High;
        for(int i = 0;i<HandValues.Length; i++)
        {
            HandValues[i] = 0;
        }
    }
    public HandType HandType;
    public int[] HandValues = new int[5];
    public int Value()
    {
        return (int)HandType;
    }
}
class Hand
{
    public Hand(List<Card> _cardss)
    {
        this.cards = _cardss.ToArray();
    }

    Card[] cards = new Card[5];

    public void WriteCards()
    {
        foreach (Card card in cards)
        {
            Console.WriteLine(card.type.ToString() + " " + (card.id + 1).ToString());
        }
    }

    public int Compare(Hand _hand)
    {
        HandValue _handValue1;
        HandValue _handValue2;

        _handValue1 = this.CheckCards();
        _handValue2 = _hand.CheckCards();

        if(_handValue1.HandType > _handValue2.HandType)
        {
            return 1;
        }
        else if(_handValue1.HandType < _handValue2.HandType)
        {
            return 0;
        }
        else
        {
            for(int i = 0;i < 5; i++)
            {
                if (_handValue1.HandValues[i] > _handValue2.HandValues[i])
                {
                    return 1;
                }
                else if(_handValue1.HandValues[i] < _handValue2.HandValues[i])
                {
                    return 0;
                }
            }
            return 2;
        }
    }

    public HandValue CheckCards()
    {
        for (int i = 0; i < 5; i++)
        {
            cards[i].check = false;
        }
           
        return Pair(cards.ToList());
        
    }

    

    HandValue Pair(List<Card> _cards)
    {
        HandValue _handValue1 = new HandValue();

        int[] cardid = new int[5];
        bool[] cardRemove = new bool[5];
        for(int i = 0;i< 5; i++)
        {
            cardid[i] = _cards[i].id;
        }
        List<int> cardVals = new List<int>();
        List<int> cardValsPair = new List<int>();

        int[] sortedCardValues = new int[5];

        /*
        for(int i = 0; i < cards.Length; i++)
        {
            sortedCardValues[i] = cards[i].id;
        }

        Array.Sort(sortedCardValues);

        Array.Reverse(sortedCardValues);
        */


        //Pairs
        int[] counts = new int[5];
        for (int i = 0; i < cards.Length; i++)
        {
            for (int j = i + 1; j < cards.Length; j++)
            {
                if (_cards[i].id == _cards[j].id && !_cards[j].check)
                {
                    counts[i]++;
                    _cards[j].check = true;
                    cardRemove[i] = true;
                    cardRemove[j] = true;
                    
                }
            }
            _cards[i].check = true;
        }

        for(int i = 0; i < 5; i++)
        {
            if (cardRemove[i] == true)
            {
                cardValsPair.Add(cardid[i]);
            }
            else
            {
                cardVals.Add(cardid[i]);
            }
        }

        for (int i = 0; i < cardValsPair.Count; i++)
        {
            int[] v = cardValsPair.ToArray();
            Array.Sort(v);
            Array.Reverse(v);
            sortedCardValues[i] = v[i];
        }

        for (int i = 0; i < cardVals.Count; i++)
        {
            int[] v = cardVals.ToArray();
            Array.Sort(v);
            Array.Reverse(v);
            sortedCardValues[i + cardValsPair.Count] = v[i];
        }


        Array.Sort(counts);
        Array.Reverse(counts);
        if (counts[0] == 0)
        {
            
        }
        else
        {
            if (counts[0] == 1)
            {
                if (counts[1] == 1)
                {
                    _handValue1.HandType = HandType.TwoPair;
                }
                else
                {
                    _handValue1.HandType = HandType.Pair;
                }
            }
            else
            {
                if (counts[0] == 2)
                {
                    if (counts[1] == 1)
                    {
                        _handValue1.HandType = HandType.FullHouse;
                    }
                    else
                    {
                        _handValue1.HandType = HandType.Three;
                    }
                }
                else
                {
                    _handValue1.HandType = HandType.Four;
                }
            }
        }

        //Straights and Flushes

        HandValue _handValue2 = new HandValue();

        Card[] c = cards;
        bool str = false;
        bool fls = false;

        if (cards[0].type == cards[1].type && cards[1].type == cards[2].type && cards[2].type == cards[3].type && cards[3].type == cards[4].type)
        {
            fls = true;
        }

        Array.Sort(c);

        if (c[0].id + 1 == c[1].id && c[1].id + 1 == c[2].id && c[2].id + 1 == c[3].id && c[3].id + 1 == c[4].id)
        {
            str = true;
        }

        if (str)
        {
            if (fls)
            {
                _handValue2.HandType = HandType.StraightFlush;
            }
            else
            {
                _handValue2.HandType = HandType.Straight;
            }
        }
        else
        {
            if (fls)
            {
                _handValue2.HandType = HandType.Flush;
            }
        }

        _handValue1.HandValues = sortedCardValues;
        _handValue2.HandValues = sortedCardValues;

        if((int)_handValue1.HandType > (int)_handValue2.HandType)
        {
            return _handValue1;
        }
        else if((int)_handValue1.HandType == (int)_handValue2.HandType)
        {
            return _handValue1;
        }
        else
        {
            return _handValue2;
        }

            
    }
}

class Deck
{
    public Deck(int _seed) { seed = _seed; }

    int seed;
    int seedInc = 0;

    public List<Card> cards = new List<Card>();
    public void CreateDeck()
    {
        for (int t = 0; t < 4; t++)
        {
            for (int d = 0; d < 13; d++)
            {
                cards.Add(new Card(d, t));
            }
        }

    }

    public Card Draw()
    {
        seedInc++;
        Card c = cards[(int)MathF.Floor((cards.Count - 1) * HashNew(seed + (seedInc - 1) * 9))];
        cards.RemoveAt((int)MathF.Floor((cards.Count - 1) * HashNew(seed + (seedInc - 1) * 9)));
        return c;
    }

    public Card Peek()
    {
        return cards[(int)MathF.Floor((cards.Count - 1) * Hash(seed + seedInc * 9))];
    }

    float Hash(int _in)
    {
        float _out = _in * 0.1031f - MathF.Floor(_in * 0.1031f);
        _out *= _out + 33;
        _out *= _out + _out;
        return _out - MathF.Floor(_out);
    }

    float HashNew(int _seed)
    {
        Random rnd = new Random();
        return (float)rnd.NextDouble();
    }

}

class Card : IComparable
{

    public int CompareTo(object obj)
    {
        // throws invalid cast exception if not of type Donator
        Card otherCard = (Card)obj;

        return this.id.CompareTo(otherCard.id);
    }

    public Card()
    {

    }

    public Card(int _id, int _type)
    {
        id = _id;
        type = (CardType)_type;
    }

    public int id; //min 0 max 12 - a 2 3 4 5 6 7 8 9 10 j q k -
    public CardType type;//4 types - H D C S -
    public bool check = false;

    public int value()
    {
        if (id > 0)
        {
            return id + 1;
        }
        else
        {
            return 13;
        }
    }

    bool BiggerThan(Card _card)
    {
        if (_card == null || this == null)
        {
            return false;
        }
        if (_card.value() < this.value())
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}

enum CardType
{
    Hearts,
    Diamonds,
    Clubs,
    Spades
}



using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Threading;


int rep = 50000;
int handCount = 0;
HandType h = HandType.High;
int money = 1000;
int currentBet = 0;


for (int i = 0; i < rep; i++)
{
    Hand hand;
    List<Card> icards = new List<Card>();
    Deck deck = new Deck( i /*Convert.ToInt32(Console.ReadLine())*/);
    Console.Clear();
    deck.CreateDeck();
    icards.Add(deck.Draw());
    icards.Add(deck.Draw());
    icards.Add(deck.Draw());
    icards.Add(deck.Draw());
    icards.Add(deck.Draw());

    hand = new Hand(icards);

    icards.Clear();

    icards.Add(deck.Draw());
    icards.Add(deck.Draw());
    icards.Add(deck.Draw());
    icards.Add(deck.Draw());
    icards.Add(deck.Draw());

    Hand compHand = new Hand(icards);

    int comp = hand.Compare(compHand);

    hand.WriteCards();

    Console.WriteLine();
    Console.WriteLine("---");
    Console.WriteLine();

    Console.WriteLine("Type : " + hand.CheckCards().HandType);

    Console.WriteLine();
    Console.WriteLine("---");
    Console.WriteLine();

    compHand.WriteCards();

    Console.WriteLine();
    Console.WriteLine("---");
    Console.WriteLine();

    Console.WriteLine("Type : " + compHand.CheckCards().HandType);

    Console.WriteLine();
    Console.WriteLine("---");
    Console.WriteLine();

    Console.WriteLine("Winning Hand : " + comp.ToString());

    

    if(hand.CheckCards().HandType==h || compHand.CheckCards().HandType == h )
    {
        int reservedint = 0;
    }

    if (hand.CheckCards().HandType == h )
    {
        handCount++;
    }

    Console.Clear();
}

enum HandType {High,Pair,TwoPair,Three,Straight,Flush,FullHouse,Four,StraightFlush }

 class HandValue
{
    public HandValue()
    {
        HandType = HandType.High;
        for(int i = 0;i<HandValues.Length; i++)
        {
            HandValues[i] = 0;
        }
    }
    public HandType HandType;
    public int[] HandValues = new int[5];
    public int Value()
    {
        return (int)HandType;
    }
}
class Hand
{
    public Hand(List<Card> _cardss)
    {
        this.cards = _cardss.ToArray();
    }

    Card[] cards = new Card[5];

    public void WriteCards()
    {
        foreach (Card card in cards)
        {
            Console.WriteLine(card.type.ToString() + " " + (card.id + 1).ToString());
        }
    }

    public int Compare(Hand _hand)
    {
        HandValue _handValue1;
        HandValue _handValue2;

        _handValue1 = this.CheckCards();
        _handValue2 = _hand.CheckCards();

        if(_handValue1.HandType > _handValue2.HandType)
        {
            return 1;
        }
        else if(_handValue1.HandType < _handValue2.HandType)
        {
            return 0;
        }
        else
        {
            for(int i = 0;i < 5; i++)
            {
                if (_handValue1.HandValues[i] > _handValue2.HandValues[i])
                {
                    return 1;
                }
                else if(_handValue1.HandValues[i] < _handValue2.HandValues[i])
                {
                    return 0;
                }
            }
            return 2;
        }
    }

    public HandValue CheckCards()
    {
        for (int i = 0; i < 5; i++)
        {
            cards[i].check = false;
        }
           
        return Pair(cards.ToList());
        
    }

    

    HandValue Pair(List<Card> _cards)
    {
        HandValue _handValue1 = new HandValue();

        int[] cardid = new int[5];
        bool[] cardRemove = new bool[5];
        for(int i = 0;i< 5; i++)
        {
            cardid[i] = _cards[i].id;
        }
        List<int> cardVals = new List<int>();
        List<int> cardValsPair = new List<int>();

        int[] sortedCardValues = new int[5];

        /*
        for(int i = 0; i < cards.Length; i++)
        {
            sortedCardValues[i] = cards[i].id;
        }

        Array.Sort(sortedCardValues);

        Array.Reverse(sortedCardValues);
        */


        //Pairs
        int[] counts = new int[5];
        for (int i = 0; i < cards.Length; i++)
        {
            for (int j = i + 1; j < cards.Length; j++)
            {
                if (_cards[i].id == _cards[j].id && !_cards[j].check)
                {
                    counts[i]++;
                    _cards[j].check = true;
                    cardRemove[i] = true;
                    cardRemove[j] = true;
                    
                }
            }
            _cards[i].check = true;
        }

        for(int i = 0; i < 5; i++)
        {
            if (cardRemove[i] == true)
            {
                cardValsPair.Add(cardid[i]);
            }
            else
            {
                cardVals.Add(cardid[i]);
            }
        }

        for (int i = 0; i < cardValsPair.Count; i++)
        {
            int[] v = cardValsPair.ToArray();
            Array.Sort(v);
            Array.Reverse(v);
            sortedCardValues[i] = v[i];
        }

        for (int i = 0; i < cardVals.Count; i++)
        {
            int[] v = cardVals.ToArray();
            Array.Sort(v);
            Array.Reverse(v);
            sortedCardValues[i + cardValsPair.Count] = v[i];
        }


        Array.Sort(counts);
        Array.Reverse(counts);
        if (counts[0] == 0)
        {
            
        }
        else
        {
            if (counts[0] == 1)
            {
                if (counts[1] == 1)
                {
                    _handValue1.HandType = HandType.TwoPair;
                }
                else
                {
                    _handValue1.HandType = HandType.Pair;
                }
            }
            else
            {
                if (counts[0] == 2)
                {
                    if (counts[1] == 1)
                    {
                        _handValue1.HandType = HandType.FullHouse;
                    }
                    else
                    {
                        _handValue1.HandType = HandType.Three;
                    }
                }
                else
                {
                    _handValue1.HandType = HandType.Four;
                }
            }
        }

        //Straights and Flushes

        HandValue _handValue2 = new HandValue();

        Card[] c = cards;
        bool str = false;
        bool fls = false;

        if (cards[0].type == cards[1].type && cards[1].type == cards[2].type && cards[2].type == cards[3].type && cards[3].type == cards[4].type)
        {
            fls = true;
        }

        Array.Sort(c);

        if (c[0].id + 1 == c[1].id && c[1].id + 1 == c[2].id && c[2].id + 1 == c[3].id && c[3].id + 1 == c[4].id)
        {
            str = true;
        }

        if (str)
        {
            if (fls)
            {
                _handValue2.HandType = HandType.StraightFlush;
            }
            else
            {
                _handValue2.HandType = HandType.Straight;
            }
        }
        else
        {
            if (fls)
            {
                _handValue2.HandType = HandType.Flush;
            }
        }

        _handValue1.HandValues = sortedCardValues;
        _handValue2.HandValues = sortedCardValues;

        if((int)_handValue1.HandType > (int)_handValue2.HandType)
        {
            return _handValue1;
        }
        else if((int)_handValue1.HandType == (int)_handValue2.HandType)
        {
            return _handValue1;
        }
        else
        {
            return _handValue2;
        }

            
    }
}

class Deck
{
    public Deck(int _seed) { seed = _seed; }

    int seed;
    int seedInc = 0;

    public List<Card> cards = new List<Card>();
    public void CreateDeck()
    {
        for (int t = 0; t < 4; t++)
        {
            for (int d = 0; d < 13; d++)
            {
                cards.Add(new Card(d, t));
            }
        }

    }

    public Card Draw()
    {
        seedInc++;
        Card c = cards[(int)MathF.Floor((cards.Count - 1) * HashNew(seed + (seedInc - 1) * 9))];
        cards.RemoveAt((int)MathF.Floor((cards.Count - 1) * HashNew(seed + (seedInc - 1) * 9)));
        return c;
    }

    public Card Peek()
    {
        return cards[(int)MathF.Floor((cards.Count - 1) * Hash(seed + seedInc * 9))];
    }

    float Hash(int _in)
    {
        float _out = _in * 0.1031f - MathF.Floor(_in * 0.1031f);
        _out *= _out + 33;
        _out *= _out + _out;
        return _out - MathF.Floor(_out);
    }

    float HashNew(int _seed)
    {
        Random rnd = new Random();
        return (float)rnd.NextDouble();
    }

}

class Card : IComparable
{

    public int CompareTo(object obj)
    {
        // throws invalid cast exception if not of type Donator
        Card otherCard = (Card)obj;

        return this.id.CompareTo(otherCard.id);
    }

    public Card()
    {

    }

    public Card(int _id, int _type)
    {
        id = _id;
        type = (CardType)_type;
    }

    public int id; //min 0 max 12 - a 2 3 4 5 6 7 8 9 10 j q k -
    public CardType type;//4 types - H D C S -
    public bool check = false;

    public int value()
    {
        if (id > 0)
        {
            return id + 1;
        }
        else
        {
            return 13;
        }
    }

    bool BiggerThan(Card _card)
    {
        if (_card == null || this == null)
        {
            return false;
        }
        if (_card.value() < this.value())
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}

enum CardType
{
    Hearts,
    Diamonds,
    Clubs,
    Spades
}




