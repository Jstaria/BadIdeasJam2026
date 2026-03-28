
using AYellowpaper.SerializedCollections;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UIElements;

public enum HanoiAttribute
{
    Number,
    Color,
    Sides
}

public class HanoiPuzzle : MonoBehaviour
{
    [SerializeField] private HanoiPiece[] pieces;
    [SerializeField] private Transform[] pegTransforms;

    [SerializeField] private UnityEvent OnDone;

    [SerializedDictionary("Puzzle","LightMat")]
    [SerializeField] private SerializedDictionary<HanoiAttribute, SwapMaterial> lights;

    private Dictionary<int, Stack<HanoiPiece>> pegs = new Dictionary<int, Stack<HanoiPiece>>();
    private bool isPlacing;
    private HanoiPiece heldPiece;
    private Coroutine heldPieceMoveCoroutine;

    private bool solvedNumber;
    private bool solvedColor;
    private bool solvedSides;

    private HanoiAttribute currentAttribute;

    private void Awake()
    {
        pegs = new Dictionary<int, Stack<HanoiPiece>>
        {
            { 0, new Stack<HanoiPiece>() },
            { 1, new Stack<HanoiPiece>() },
            { 2, new Stack<HanoiPiece>() },
            { 3, new Stack<HanoiPiece>() }
        };

        foreach (HanoiPiece piece in pieces)
        {
            pegs[3].Push(piece);
        }
    }

    private void Update()
    {
        for (int i = 0; i < pegs.Count; i++)
        {
            if (!pegs.ContainsKey(i)) continue;

            List<HanoiPiece> pieces = pegs[i].ToList();

            if (pieces.Count != 4) continue;

            if (!solvedNumber && (solvedNumber = IsOrdered(pieces, p => p.Number)))
            {
                Debug.Log($"Peg {i} solved for Number!");
                lights[currentAttribute].Switch();
                currentAttribute = HanoiAttribute.Sides;
            }

            if (!solvedColor && (solvedColor = IsOrdered(pieces, p => p.Color)))
            {
                Debug.Log($"Peg {i} solved for Color!");
                lights[currentAttribute].Switch();
                OnDone?.Invoke();
            }

            if (!solvedSides && (solvedSides = IsOrdered(pieces, p => p.Sides)))
            {
                Debug.Log($"Peg {i} solved for Sides!");
                lights[currentAttribute].Switch();
                currentAttribute = HanoiAttribute.Color;
            }
        }
    }

    private bool IsOrdered(List<HanoiPiece> pieces, Func<HanoiPiece, int> selector)
    {
        for (int i = 0; i < pieces.Count; i++)
        {
            int expected = i + 1;

            if (selector(pieces[i]) != expected)
                return false;
        }

        return true;
    }

    private bool CheckOrder(HanoiPiece heldPiece, int peg)
    {
        bool canPlace = false;

        if (pegs[peg].Count == 0)
        {
            canPlace = true;
        }
        else
        {
            HanoiPiece topPiece = pegs[peg].Peek();

            switch (currentAttribute)
            {
                case HanoiAttribute.Number:
                    if (heldPiece.Number < topPiece.Number)
                        canPlace = true;
                    break;
                case HanoiAttribute.Color:
                    if (heldPiece.Color < topPiece.Color)
                        canPlace = true;
                    break;
                case HanoiAttribute.Sides:
                    if (heldPiece.Sides < topPiece.Sides)
                        canPlace = true;
                    break;
            }
        }

        return canPlace;
    }

    public void OnInteract(int peg)
    {
        if (isPlacing) OnPlace(peg);
        else OnPickup(peg);
    }

    public void OnPlace(int peg)
    {
        if (!CheckOrder(heldPiece, peg)) return;

        Vector3 position = pegTransforms[peg].localPosition;
        position.y = -1 + pegs[peg].Count() * .85f;

        if (heldPieceMoveCoroutine != null)
            StopCoroutine(heldPieceMoveCoroutine);
        heldPieceMoveCoroutine = StartCoroutine(MovePegs(heldPiece.transform, position));

        pegs[peg].Push(heldPiece);

        heldPiece = null;
        isPlacing = false;
    }

    public void OnPickup(int peg)
    {
        if (pegs[peg].Count == 0) return;

        heldPiece = pegs[peg].Pop();

        Vector3 position = heldPiece.transform.localPosition;
        position.y = 3;

        if (heldPieceMoveCoroutine != null)
            StopCoroutine(heldPieceMoveCoroutine);
        heldPieceMoveCoroutine = StartCoroutine(MoveTo(heldPiece.transform, position));

        isPlacing = true;
    }

    private IEnumerator MovePegs(Transform heldPiece, Vector3 position)
    {
        Vector3 upPosition = position;
        upPosition.y = 3;

        yield return (StartCoroutine(MoveTo(heldPiece, upPosition)));
        yield return (StartCoroutine(MoveTo(heldPiece, position)));
    }

    private IEnumerator MoveTo(Transform heldPiece, Vector3 position)
    {
        float time = .25f;
        Vector3 startPos = heldPiece.transform.localPosition;

        while (time > 0)
        {
            time -= Time.deltaTime;

            float lerp = time / .25f;

            heldPiece.localPosition = Vector3.Lerp(position, startPos, lerp);

            yield return null;
        }
    }
}
