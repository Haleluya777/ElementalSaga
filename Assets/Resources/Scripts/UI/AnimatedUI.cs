using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Hallelujah;

public class AnimatedUI : MonoBehaviour
{
    public bool isAnimate;
    [SerializeField] private Image img;
    [SerializeField] private List<Sprite> sprites = new List<Sprite>();
    private CirclularList<Sprite> animations;
    private float timer = 0;
    private float interval;

    private void Start()
    {
        //img = GetComponent<Image>();
        interval = .02f;
        animations = new CirclularList<Sprite>(sprites);
    }

    private void Update()
    {
        timer += Time.deltaTime;
        if (isAnimate)
        {
            if (timer >= interval)
            {
                timer = 0;
                img.sprite = animations.Get();
                animations.Next();
            }
        }
    }
}
