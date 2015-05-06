var Slider = {

    slider: $('.extension-slider'),
    item: $('.slider-pic', this.slider),
    listItem: $('.slider-scroll .list-item'),
    count: null,
    leftCtrl: $('.__ctrl-l'),
    rightCtrl: $('.__ctrl-r'),
    visibleItems: null,
    index: 0,
    
    init: function() {
        var sliderScrollWidth, listItemWidth;

        sliderScrollWidth = $('.slider-scroll', this.slider).width();
        listItemWidth = this.listItem.width() + 12;

        $('.slider-scroll .list').css({'margin-left': '-' + parseInt(this.index * listItemWidth) + 'px'});

        this.visibleItems = parseInt(sliderScrollWidth / listItemWidth);

        this.events();
    },

    events: function() {
        var self = this;
        this.count = $('.list-item', this.slider).length;

        var clicked;
     
        clicked = function(e) {
            e.preventDefault();
            pos = e.data.pos;

            if (pos == 'left') {
                if(self.index > 0) {
                    self.index--;

                    self.leftCtrl.off('click');

                    self.listItem.removeClass('__selected').eq(self.index).addClass('__selected');
                    self.item.hide().eq(self.index).show();

                    $('.list', self.slider).animate({'margin-left': '+=' + parseInt(self.listItem.width() + 12) + 'px'}, 500, function () {
                        self.leftCtrl.off('click').on('click', {pos: 'left'}, clicked);
                    });
                }
            }
            else {
                if(self.index < self.count - self.visibleItems) {
                    self.index++;

                    self.listItem.removeClass('__selected').eq(self.index).addClass('__selected');
                    self.item.hide().eq(self.index).show();

                    self.rightCtrl.off('click');

                    $('.list', self.slider).animate({'margin-left': '-=' + parseInt(self.listItem.width() + 12) + 'px'}, 500, function () {
                        self.rightCtrl.off('click').on('click', {pos: 'right'}, clicked);
                    });
                }
            }
        };

        this.listItem.on('click', function () {
            var index = $(this).index();

            $('.list-item').removeClass('__selected').eq(index).addClass('__selected');
            self.item.hide().eq(index).show();
            console.log(index)

        });

        this.leftCtrl.off('click').on('click', {pos: 'left'}, clicked);
        this.rightCtrl.off('click').on('click', {pos: 'right'}, clicked);
    }

}

$(function () {
    $('.slider-pic:not(:first)').hide();
    $('.slider-scroll .list-item:first').addClass('__selected');

    Slider.init();

    $(window).resize(function () {
        Slider.init();
    });
});