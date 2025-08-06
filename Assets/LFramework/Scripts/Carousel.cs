using System.Collections;
using System.Collections.Generic;

#if DG_Installed
using DG.Tweening;
#endif
using LFramework;
using UnityEngine;
using UnityEngine.UI;


public class Carousel : MonoBehaviour
{
    public RawImage rawImage1;
    public RawImage rawImage2;

    public int imageIndex;

    public Transform defaultPosition1;
    public Transform defaultPosition2;
    public List<Texture2D> images = new List<Texture2D>();


    private void Awake()
    {
        moveSpeed = IniTool.GetValue("轮播", "移动速度", Application.streamingAssetsPath + "/Config.txt", "1").ToFloat();
        isAutoOpenAndMove = IniTool.GetValue("轮播", "是否轮播", Application.streamingAssetsPath + "/Config.txt", "false").ToBool();
        AutoDuration = IniTool.GetValue("轮播", "切换时间", Application.streamingAssetsPath + "/Config.txt", "5").ToFloat();
        waitAuto = IniTool.GetValue("轮播", "触摸后等待多少时间自动", Application.streamingAssetsPath + "/Config.txt", "10").ToFloat();

        right.onClick.RemoveAllListeners();
        left.onClick.RemoveAllListeners();
        right.onClick.AddListener(() =>
        {
            // 如果轮播的协程存在 就停止
            if (autoNextImageCoroutine != null)
            {
                StopCoroutine(autoNextImageCoroutine);
            }

            // 如果 等待自动轮播的协程存在 就停止
            if (waitToAutoImageCoroutine != null)
            {
                StopCoroutine(waitToAutoImageCoroutine);
            }

            // 开启等待自动轮播
            waitToAutoImageCoroutine = StartCoroutine(BtnClickWaitToAuto());
            // 调用下一张
            NextImg();
        });
        left.onClick.AddListener(() =>
        {
            // 如果轮播的协程存在 就停止
            if (autoNextImageCoroutine != null)
            {
                StopCoroutine(autoNextImageCoroutine);
            }

            // 如果 等待自动轮播的协程存在 就停止
            if (waitToAutoImageCoroutine != null)
            {
                StopCoroutine(waitToAutoImageCoroutine);
            }

            // 开启等待自动轮播
            waitToAutoImageCoroutine = StartCoroutine(BtnClickWaitToAuto());
            PreImg();
        });
    }


    private IEnumerator BtnClickWaitToAuto()
    {
        // 等待   触摸后等待多少时间自动 的时间
        yield return new WaitForSeconds(waitAuto);
        // 如果 轮播的协程存在 就停止
        if (autoNextImageCoroutine != null)
        {
            StopCoroutine(autoNextImageCoroutine);
        }

        // 开启自动轮播
        if (isAutoOpenAndMove)
        {
            autoNextImageCoroutine = StartCoroutine(AutoNextImage());
        }
    }


    public bool isOnEnableToPlay;
    public bool isAutoOpenAndMove;
    public bool loop;
    public Button left;
    public Button right;

    private void OnEnable()
    {
        if (isOnEnableToPlay)
        {
            Play();
        }
    }


    public void Play()
    {
        // StopCoroutine("AutoNextImage");


        // images.Clear();
        // images.AddRange(list);

        imageIndex = 0;
#if DG_Installed
        DOTween.Kill("one");
#endif
        next = true;
        changeTime = 0;
        moving = false;
        rawImage1.transform.localPosition = defaultPosition1.localPosition;
        rawImage2.transform.localPosition = defaultPosition2.localPosition;

        rawImage1.texture = images[index: 0];


        if (images.Count <= 1)
        {
            rawImage2.texture = rawImage1.texture;
        }
        else
        {
            rawImage2.texture = images[1];
        }

        SetImageSize(rawImage1);
        SetImageSize(rawImage2);

        if (autoNextImageCoroutine != null)
        {
            StopCoroutine(autoNextImageCoroutine);
        }

        if (isAutoOpenAndMove)
        {
            autoNextImageCoroutine = StartCoroutine(AutoNextImage());
        }

        CheckBtn();
    }

    public void Play(List<Texture2D> list)
    {
        // 如果轮播的协程存在 就停止
        if (autoNextImageCoroutine != null)
        {
            StopCoroutine(autoNextImageCoroutine);
        }

        // 如果 等待自动轮播的协程存在 就停止
        if (waitToAutoImageCoroutine != null)
        {
            StopCoroutine(waitToAutoImageCoroutine);
        }


        images.Clear();
        images.AddRange(list);

        imageIndex = 0;
#if DG_Installed
        DOTween.Kill("one");
#endif
        next = true;
        changeTime = 0;
        rawImage1.transform.localPosition = defaultPosition1.localPosition;
        rawImage2.transform.localPosition = defaultPosition2.localPosition;

        rawImage1.texture = images[index: 0];


        if (images.Count <= 1)
        {
            rawImage2.texture = rawImage1.texture;
        }
        else
        {
            rawImage2.texture = images[1];
        }

        SetImageSize(rawImage1);
        SetImageSize(rawImage2);


        // 如果轮播的协程存在 就停止
        if (autoNextImageCoroutine != null)
        {
            StopCoroutine(autoNextImageCoroutine);
        }

        // 如果 等待自动轮播的协程存在 就停止
        if (waitToAutoImageCoroutine != null)
        {
            StopCoroutine(waitToAutoImageCoroutine);
        }

        if (isAutoOpenAndMove)
        {
            autoNextImageCoroutine = StartCoroutine(AutoNextImage());
        }


        CheckBtn();
    }

    public void RePlay()
    {
        // 如果轮播的协程存在 就停止
        if (autoNextImageCoroutine != null)
        {
            StopCoroutine(autoNextImageCoroutine);
        }

        // 如果 等待自动轮播的协程存在 就停止
        if (waitToAutoImageCoroutine != null)
        {
            StopCoroutine(waitToAutoImageCoroutine);
        }


        imageIndex = 0;
#if DG_Installed
        DOTween.Kill("one");
#endif
        moving = false;
        next = true;
        changeTime = 0;
        rawImage1.transform.localPosition = defaultPosition1.localPosition;
        rawImage2.transform.localPosition = defaultPosition2.localPosition;

        rawImage1.texture = images[index: 0];


        if (images.Count <= 1)
        {
            rawImage2.texture = rawImage1.texture;
        }
        else
        {
            rawImage2.texture = images[1];
        }

        SetImageSize(rawImage1);
        SetImageSize(rawImage2);


        // 如果轮播的协程存在 就停止
        if (autoNextImageCoroutine != null)
        {
            StopCoroutine(autoNextImageCoroutine);
        }

        // 如果 等待自动轮播的协程存在 就停止
        if (waitToAutoImageCoroutine != null)
        {
            StopCoroutine(waitToAutoImageCoroutine);
        }

        if (isAutoOpenAndMove)
        {
            autoNextImageCoroutine = StartCoroutine(AutoNextImage());
        }


        CheckBtn();
    }

    public float AutoDuration = 5;
    public bool next = true;
    public float moveSpeed = 0.5f;

    private float waitAutoTemp;
    private float waitAuto;

    public Coroutine autoNextImageCoroutine;
    public Coroutine waitToAutoImageCoroutine;

    private IEnumerator AutoNextImage()
    {
        while (true)
        {
            yield return new WaitForSeconds(AutoDuration);
            if (next)
            {
                NextImg();
            }
            else
            {
                PreImg();
            }
        }
    }


    public int changeTime;

    public void NextImg()
    {
        CheckBtn();
        print("移动前:" + imageIndex);
        if (images.Count <= 1)
        {
            // PlayVideo();
            // imageIndex = 0;
            // StopAllCoroutines();
            // GoImage(0);
            return;
        }

        if (moving)
        {
            return;
        }

        changeTime++;
        imageIndex++;
        //是最后一个图
        if (imageIndex >= images.Count)
        {
            if (loop)
            {
                imageIndex = 0;
            }
            else
            {
                // 开始播放视频
                //PlayVideo();
                imageIndex = images.Count - 1;
                // GoImage(0);
                // StopAllCoroutines();
                return;
            }
        }

        print("不是最后一个图");
        CheckBtn();
        moving = true;
        if (changeTime % 2 == 1)
        {
            rawImage2.texture = images[imageIndex];
            SetImageSize(rawImage2);
            var localPosition = defaultPosition2.localPosition;
            rawImage2.transform.localPosition = localPosition;
            rawImage1.transform.localPosition = defaultPosition1.localPosition;
#if DG_Installed
            rawImage1.transform.DOLocalMoveX(-localPosition.x, moveSpeed).OnComplete
            (
                () =>
                {
                    moving = false;
                    rawImage1.transform.localPosition = new Vector3(0, 10240, 0);
                }
            ).SetId("one");
            rawImage2.transform.DOLocalMoveX(0, moveSpeed).SetId("one");
#endif
        }
        else if (changeTime % 2 == 0)
        {
            rawImage1.texture = images[imageIndex];
            SetImageSize(rawImage1);
            var localPosition = defaultPosition2.localPosition;
            rawImage1.transform.localPosition = localPosition;
            rawImage2.transform.localPosition = defaultPosition1.localPosition;
#if DG_Installed
            rawImage2.transform.DOLocalMoveX(-localPosition.x, moveSpeed).OnComplete
            (
                () =>
                {
                    moving = false;
                    rawImage2.transform.localPosition = new Vector3(0, 10240, 0);
                }
            ).SetId("one");
            rawImage1.transform.DOLocalMoveX(0, moveSpeed).SetId("one");
#endif
        }
    }

    private void CheckBtn()
    {
        if (images.Count <= 1)
        {
            right.interactable = false;
            left.interactable = false;
        }
        else
        {
            if (loop)
            {
                right.interactable = true;
                left.interactable = true;
            }
            else
            {
                if (imageIndex >= images.Count - 1)
                {
                    right.interactable = false;
                    left.interactable = true;
                }
                else if (imageIndex <= 0)
                {
                    left.interactable = false;
                    right.interactable = true;
                }
                else
                {
                    left.interactable = true;
                    right.interactable = true;
                }
            }
        }
    }


    [SerializeField] private bool moving;

    public void PreImg()
    {
        CheckBtn();
        if (images.Count <= 1)
        {
            return;
        }


        if (moving)
        {
            return;
        }

        //第一个图
        if (imageIndex <= 0)
        {
            if (loop)
            {
                imageIndex = images.Count;
            }
            else
            {
                imageIndex = 0;
                //NextImg();
                //next = true;
                return;
            }
        }

        // 不是第一个图
        moving = true;
        changeTime++;
        imageIndex--;
        CheckBtn();
        if (changeTime % 2 == 1)
        {
            rawImage2.texture = images[imageIndex];

            // SetImageSize(rawImage2);
            var localPosition = defaultPosition2.localPosition;
            rawImage2.transform.localPosition = new Vector3(-localPosition.x, 0, 0);
            rawImage1.transform.localPosition = defaultPosition1.localPosition;
#if DG_Installed
            rawImage1.transform.DOLocalMoveX(localPosition.x, moveSpeed).OnComplete
            (
                () =>
                {
                    moving = false;
                    rawImage1.transform.localPosition = new Vector3(0, 10240, 0);
                }
            ).SetId("one");
            rawImage2.transform.DOLocalMoveX(0, moveSpeed).SetId("one");
#endif
        }
        else if (changeTime % 2 == 0)
        {
            rawImage1.texture = images[imageIndex];
            // SetImageSize(rawImage1);
            var localPosition = defaultPosition2.localPosition;
            rawImage1.transform.localPosition = new Vector3(-localPosition.x, 0, 0);
            rawImage2.transform.localPosition = defaultPosition1.localPosition;
#if DG_Installed
            rawImage2.transform.DOLocalMoveX(localPosition.x, moveSpeed).OnComplete
            (
                () =>
                {
                    moving = false;
                    rawImage2.transform.localPosition = new Vector3(0, 10240, 0);
                }
            ).SetId("one");
            rawImage1.transform.DOLocalMoveX(0, moveSpeed).SetId("one");
#endif
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            PreImg();
        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            NextImg();
        }
    }

    public float weight1 = 1920.0f;
    public float heght1 = 1080.0f;

    public void SetImageSize(RawImage raw, Texture2D texture)
    {
        // print(texture.width+"dfdf"+texture.height);
        if ((float)texture.width / (float)texture.height >= weight1 / heght1)
        {
            raw.rectTransform.sizeDelta = new Vector2
                (weight1, weight1 / ((float)texture.width / (float)texture.height));
        }
        else
        {
            raw.rectTransform.sizeDelta = new Vector2(heght1 * ((float)texture.width / (float)texture.height), heght1);
        }
    }

    public void SetImageSize(RawImage raw)
    {
        return;
        //print(raw.texture.name);

        if ((float)raw.texture.width / (float)raw.texture.height >= weight1 / heght1)
        {
            raw.rectTransform.sizeDelta = new Vector2
                (weight1, weight1 / ((float)raw.texture.width / (float)raw.texture.height));
        }
        else
        {
            raw.rectTransform.sizeDelta = new Vector2
                (heght1 * ((float)raw.texture.width / (float)raw.texture.height), heght1);
        }
    }

    // public void GoImage(int index)
    // {
    //     imageIndex = index;
    //     moving = false;
    //     StopAllCoroutines();
    //     DOTween.Kill("one");
    //     next = true;
    //     changeTime = 0;
    //     rawImage1.transform.localPosition = defaultPosition1.localPosition;
    //     rawImage2.transform.localPosition = defaultPosition2.localPosition;
    //
    //     rawImage1.texture = images1[imageIndex];
    //     rawImage1.transform.GetChild(0).GetComponent<RawImage>().texture = images2[imageIndex];
    //
    //     StartCoroutine(AutoNextImage());
    // }
    public void AutoMode()
    {
        left.gameObject.Show();
        right.gameObject.Show();
    }

    public void ActiveMode()
    {
        // 如果轮播的协程存在 就停止
        if (autoNextImageCoroutine != null)
        {
            StopCoroutine(autoNextImageCoroutine);
        }

        // 如果 等待自动轮播的协程存在 就停止
        if (waitToAutoImageCoroutine != null)
        {
            StopCoroutine(waitToAutoImageCoroutine);
        }


        left.gameObject.Hide();
        right.gameObject.Hide();
    }
}