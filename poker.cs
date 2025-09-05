





using System.Runtime.InteropServices;
using System.Threading;


List<Card> cards = new List<Card>();
Hand hand;

for (int i = 0; i < 1000; i++)
{
    Deck deck = new Deck(Convert.ToInt32(Console.ReadLine()));
    Console.Clear();
    deck.CreateDeck();
    cards.Add(new Card(1,1));
    cards.Add(new Card(2, 1));
    cards.Add(new Card(3, 1));
    cards.Add(new Card(4, 1));
    cards.Add(new Card(5, 1));

    hand = new Hand(cards);



    hand.WriteCards();

    Console.WriteLine(hand.CheckCards().ToString());

    Console.ReadKey();
    Console.Clear();
}


class Hand
{
    public Hand(List<Card> _cards)
    {
        this.cards = _cards.ToArray();
    }

    Card[] cards = new Card[5];

    public void WriteCards()
    {
        foreach(Card card in cards)
        {
            Console.WriteLine(card.type.ToString() + " " + (card.id + 1).ToString());
        }
    }

    public int CheckCards()
    {
        int ret = 0;
        int ret1 = Pair(cards.ToList());
        int ret2 = StraightFlush();

        if(ret1 < ret2)
        {
            ret = ret2;
        }
        else
        {
            ret = ret1;
        }

        return ret;
    }

    int StraightFlush()
    {
        Card[] c = cards;
        bool str = false;
        bool fls = false;

        if (cards[0].type == cards[1].type && cards[1].type == cards[2].type && cards[2].type == cards[3].type && cards[3].type == cards[4].type)
        {
            fls = true;
        }

        Array.Sort(c);

        if (c[0].id + 1 == c[1].id  && c[1].id + 1 == c[2].id && c[2].id + 1 == c[3].id && c[3].id + 1 == c[4].id)
        {
            str = true;
        }

        if (str)
        {
            if (fls)
            {
                return 8;
            }
            return 4;
        }
        else
        {
            if (fls)
            {
                return 5;
            }
        }

        return 0;
    }

    int Pair(List<Card> _cards)
    {
        int[] counts = new int[5];
        for(int i = 0; i < cards.Length; i++)
        {
            for(int j = i+1; j < cards.Length; j++)
            {
                if (_cards[i].id == _cards[j].id && !_cards[j].check)
                {
                    counts[i]++;
                    cards[j].check = true;
                }
            }
            cards[i].check = true;
        }

        Array.Sort(counts);
        Array.Reverse(counts);
        if (counts[0] == 0)
        {
            return 0;
        }
        else
        {
            if (counts[0] == 1)
            {
                if(counts[1] == 1)
                {
                    return 2;
                }
                else
                {
                    return 1;
                }
            }
            else
            {
                if (counts[0] == 2)
                {
                    if (counts[1] == 1)
                    {
                        return 6;
                    }
                    else
                    {
                        return 3;
                    }
                }
                else
                {
                    return 7;
                }
            }
        }
    }
}

class Deck
{
    public Deck(int _seed) { seed = _seed;}

    int seed;
    int seedInc = 0;

    public List<Card> cards = new List<Card>();
    public void CreateDeck()
    {
        for (int t = 0; t < 4; t++)
        {
            for(int d = 0;d<13;d++)
            {
                cards.Add(new Card(d,t));
            }
        }
        
    }

    public Card Draw()
    {
        seedInc++;
        Card c = cards[(int)MathF.Floor((cards.Count - 1) * Hash(seed + (seedInc - 1) * 9))];
        cards.RemoveAt((int)MathF.Floor((cards.Count - 1) * Hash(seed + (seedInc - 1) * 9)));
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

}

class Card : IComparable
{

    public int CompareTo(object obj)
    {
        // throws invalid cast exception if not of type Donator
        Card otherCard = (Card)obj;

        return this.id.CompareTo(otherCard.id);
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



