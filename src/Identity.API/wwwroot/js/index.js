$.fn.extend({
    loading: function () {
        $(this).attr("data-original-text", $(this).text());
        $(this).prop("disabled", true);
        $(this).html('<i class="spinner-border spinner-border-sm"></i> Loading...');
    },
    reset: function () {
        $(this).prop("disabled", false);
        $(this).text($(this).attr("data-original-text"));
    },
    disable: function (time) {
        var $this = $(this);
        if (time === 0) {
            clearInterval(timer);
            $this.attr('disabled', false);
            $this.text("Send");
            time = 60;
            return;
        }
        $this.attr('disabled', true);
        $this.text(`${time} second`);
        time--;
        var timer = setTimeout(() => {
            $this.disable(time)
        }, 1000);
    }
});