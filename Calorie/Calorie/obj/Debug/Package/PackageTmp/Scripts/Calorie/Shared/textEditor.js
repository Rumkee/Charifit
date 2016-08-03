
calorie.textEditor = function (storyTextEditorID,hiddenTextID,formID) {
    this.storyTextEditor = $('#' + storyTextEditorID);
    this.hiddenText = $('#' + hiddenTextID);
    this.form = $('#'+ formID);
    var me = this;

    this.storyTextEditor.summernote({ minHeight: 300 }, 'code', 'this is something here');

    $(function () {
        me.form.on('submit', function () {
            me.hiddenText.empty();
            me.hiddenText.val(me.storyTextEditor.summernote('code'));
        });
    });




}

