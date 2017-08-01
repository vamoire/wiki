cc.Class({
    extends: cc.Component,

    properties: {
        label: {
            default: null,
            type: cc.Label
        },
        // defaults, set visually when attaching this script to the Canvas
        text: 'Hello, World!'
    },

    // use this for initialization
    onLoad: function () {
        this.label.string = this.text;
        this.label.node.runAction(cc.sequence(
            [
                cc.scaleTo(1.0, 0.0, 0.0),
                cc.callFunc(function(){
                    cc.director.loadScene("game", null);
                })
            ]
        ));
    },

    // called every frame
    update: function (dt) {

    },
});
