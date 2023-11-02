# Counter Composer

Generate counter-sheet with reusable OOB IL to accelerate tabletop wargame prototyping.

Historical material -> Standard OOB IL (Intermedia language) -> Counter Sheet

OOB IL is represented by YAML format to keep as much as possible info. Then the designer can adjust a runtime script to set how raw data is mapped to combat value, color, image, etc. 

Due to the jam time limit, I implement only individual counter export (the format is used in Vassal and Tabletop Simulator) but don't implement whole sheet export (more friendly to printing or simulators like Zuntzu). However, it's such low-hanging fruit so I will implement them after the voting.

Sample oob is extracted from [Waterloo's wikipedia](https://en.wikipedia.org/wiki/Order_of_battle_of_the_Waterloo_campaign), see the [extraction notebook](https://gist.github.com/yiyuezhuo/6370b23864fb404f068ececf370a47ff).


## Screenshots

<img src="https://img.itch.zone/aW1hZ2UvMjM0Nzk4OS8xMzkwOTA4Ny5wbmc=/original/RIEBX6.png">
<img src="https://img.itch.zone/aW1hZ2UvMjM0Nzk4OS8xMzkwOTA4Ni5wbmc=/original/L%2Fo0gY.png">
<img src="https://img.itch.zone/aW1hZ2UvMjM0Nzk4OS8xMzkwOTA4NS5wbmc=/original/uG09Ak.png">

## TODO List

- [ ] Sheet and back-sheet export
- [ ] Refine sample OOB.
- [ ] Fix truncated full oob in the editor.
- [ ] Fix OOB truncated bug in WebGL export.

## External Lib

- Jint
- YamlDotNet
- [WebGLFileSaverForUnity](https://github.com/Nateonus/WebGLFileSaverForUnity/tree/master)

