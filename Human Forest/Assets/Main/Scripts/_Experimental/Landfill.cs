using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class History : MonoSingleton<History>
{
    public static List<Episode> history;
}

public class Episode
{

}

public interface Agent
{

}

public class God : Agent
{

}

/*

초기화만 잘 하면 할 수 있는 것: 편견 있는 상태로 시작할 것인가, 편견 없는 모든 값 중립디폴트인 상태로 시작할 것인가.

enum Category 넣는다면: 종에 따른 차별을 하는 사람들인가 아닌가.

금지조항들

*/