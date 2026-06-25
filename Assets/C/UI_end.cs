using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_end : MonoBehaviour
{
    [Header("手動拖入 UI Text")]
    public Text uiText;

    void Start()
    {
        UpdateEndText();
    }

    void UpdateEndText()
    {
        // ===== 1. 秒數轉換 =====
        int totalSeconds = Mathf.FloorToInt(AAA_font.time);

        int minutes = totalSeconds / 60;
        int seconds = totalSeconds % 60;

        string timeText = minutes + "分" + seconds + "秒";

        // ===== 2. 判斷結局文字 =====
        string finalText;

        if (AAA_font.BBOS)
        {
            finalText =
                "哇！我們打敗湊祈蓮領主了！你花費了" +
                timeText +
                "通關呢！";
        }
        else
        {
            finalText =
                "誒？就這樣嘛？要不要試試不補血通關呢？你花費了" +
                timeText +
                "通關呢！";
        }

        // ===== 3. 套用到UI =====
        uiText.text = finalText;
    }
}