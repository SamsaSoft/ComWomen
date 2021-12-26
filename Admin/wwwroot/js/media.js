var selectedMediaTypes = { image: true, audio: true, video: true };
var activeFormLanguage = 'ru';

function clearLangInputs() {
    $('.lang_nav').removeClass("active");
    $('.lang_body').attr("hidden", "");
};

function getactivetab(langs) {
    for (var item of langs) {
        if ($(`#lang_nav_${item}`).hasClass("active"))
            return item;
    }
};

function createimagefromurl(url) {
    var htmlTag = $("<img>");
    htmlTag.attr("src", url);
    htmlTag.addClass("form-control img-fluid p-3");
    return htmlTag;
}

function createaudiofromurl(url) {
    var htmlTag = $("<audio>");
    htmlTag.attr("src", url);
    htmlTag.attr("controls", "");
    htmlTag.addClass("p-3");
    return htmlTag;
}

function createvideofromurl(url) {
    var htmlTag = $("<video>");
    htmlTag.attr("src", url);
    htmlTag.attr("controls", "");
    htmlTag.addClass("form-control img-fluid p-3");
    return htmlTag;
}

function createmediafromclass(cls, url) {
    if (cls == 'image') {
        return createimagefromurl(url);
    }
    if (cls == 'audio') {
        return createaudiofromurl(url);
    }
    if (cls == 'video') {
        return createvideofromurl(url);
    }
    return null;
}