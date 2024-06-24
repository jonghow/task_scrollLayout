# task_scrollLayout
많은 데이터에 비해 적은 부하를 주며 데이터를 보여줄 수 있는 InfinityScroll View Repository 입니다. 

게임 오픈 시, ResourceData 를 로딩하며, 데이터는 task_ScrollLayout\Assets\ResourceData\ItemData 에서 제어할 수 있습니다.
반응형으로 제작하기 위해 각 슬롯은 각 행을 관리하는 Group 오브젝트가 있으며, ScrollView 내부의 ScrollRect Component 의 Width 사이즈와 내부 아이템의 Width에 따라 슬롯의 갯수를 유동적으로 생성합니다.

[결과물 ↓] 

![FlexibelScroolView_Result](https://github.com/jonghow/task_scrollLayout/assets/77612145/85c2e6d1-ffdc-48ae-9bcc-72ab9c4f6b7a)
