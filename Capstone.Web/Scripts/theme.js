"use strict";

// Theme preloader
window.onload = function () {
  setTimeout(function () {
    var preloader = document.querySelector('#theme-preloader-container');
    preloader.style.opacity = '0';
    setTimeout(function () { preloader.style.display = 'none'; }, 500);
  }, 500);
};

// Init
ready(function () {
  // Show menu handler
  document.querySelector('.nav-toggle').addEventListener('click', function () {
    var nav = document.querySelector('#nav');
    nav.style.display = 'block';
    setTimeout(function () { addClass(nav, 'nav-active'); }, 50);
  });

  // Hide menu handler
  document.querySelector('.nav-close').addEventListener('click', function () {
    var nav = document.querySelector('#nav');
    removeClass(nav, 'nav-active');
    setTimeout(function () { nav.style.display = ''; }, 500);
  });

  // Subscribe Form Submit Handler
  if (document.querySelector('#subscribe-form')) {
    document.querySelector('#subscribe-form').addEventListener('submit', function (e) {
      e.preventDefault();

      // Send Form Data to Server

      var modalTitle = 'Thank you for subscribing';
      var modalBody = '<p>You will receive new recipes first.</p>';

      updateModal(modalTitle, modalBody);
      showModal();
    });
  }

  // Contact Form Submit Handler
  if (document.querySelector('#contact-form')) {
    document.querySelector('#contact-form').addEventListener('submit', function (e) {
      e.preventDefault();

      // Send Form Data to Server

      var modalTitle = 'Thank you for message';
      var modalBody = '<p>I will contact you shortly.</p>';

      updateModal(modalTitle, modalBody);
      showModal();
    });
  }

  // Feedback Form Submit Handler
  if (document.querySelector('#feedback-form')) {
    document.querySelector('#feedback-form').addEventListener('submit', function (e) {
      e.preventDefault();

      // Send Form Data to Server

      var modalTitle = 'Thanks for the activity.';
      var modalBody = '<p>After moderation, your comment will appear.</p>';

      updateModal(modalTitle, modalBody);
      showModal();
    });
  }

  // Gallery Items Scroll Handler
  addOnWheel(document.querySelector('.gallery ul'), function(e) {
    // deltaY, detail содержат пиксели
    // wheelDelta не дает возможность узнать количество пикселей
    // onwheel || MozMousePixelScroll || onmousewheel
    var delta = e.deltaY || e.detail || e.wheelDelta;
    var elem = document.querySelector('.gallery ul');
    var li = document.querySelectorAll('.gallery ul li');
    var elemWidth = 0;
    var scroll = 0;
    var maxScroll = 0;

    // Set scroll position
    if (!elem.hasAttribute('data-scroll')) {
      elem.setAttribute('data-scroll', scroll);
    }
    scroll = elem.getAttribute('data-scroll');

    // Set max scroll
    if (!elem.hasAttribute('data-max-scroll')) {
      for (var i = 0; i < li.length; i++) {
        elemWidth += parseFloat(window.getComputedStyle(li[i]).width);
        elemWidth += parseFloat(window.getComputedStyle(li[i]).marginRight);
      }
      elem.setAttribute('data-max-scroll', elemWidth);
    }
    maxScroll = elem.getAttribute('data-max-scroll') - getInnerWidth(elem);

    // Cancel scrolling
    e.preventDefault();

    // Calc scroll position
    scroll = +scroll + delta;

    // Min scroll to left
    if (scroll <= 0) {
      elem.scrollLeft = 0;
      return;
    }

    // Max scroll to right
    if (scroll >= maxScroll ) {
      elem.scrollLeft = maxScroll;
      return;
    }

    // Set scroll position
    elem.setAttribute('data-scroll', scroll);

    // Scroll items
    elem.scrollLeft = scroll;

  });

});

/* FUNCTIONS */

/**
 * Specify a function to execute when the DOM is fully loaded.
 * @param  {Function} fn A function to execute after the DOM is ready.
 */
function ready(fn) {
  if (document.attachEvent ? document.readyState === 'complete' : document.readyState !== 'loading') {
    fn();
  } else {
    document.addEventListener('DOMContentLoaded', fn);
  }
}

/**
 * Adds the specified class(es) to each element in the set of matched elements.
 * @param {Object} el        DOM element
 * @param {String} className One or more space-separated classes to be added to the class attribute of each matched element.
 */
function addClass(el, className) {
  if (el.classList)
    el.classList.add(className);
  else
    el.className += ' ' + className;
}

/**
 * Removes the specified class(es) from each element in the set of matched elements.
 * @param  {Object} el        DOM element
 * @param  {String} className One or more space-separated classes to be added to the class attribute of each matched element.
 */
function removeClass(el, className) {
  if (el.classList)
    el.classList.remove(className);
  else
    el.className = el.className.replace(new RegExp('(^|\\b)' + className.split(' ').join('|') + '(\\b|$)', 'gi'), ' ');
}

/**
 * Shows Modal Window
 */
function showModal() {
  var modal = document.querySelector('#modal');
  // Backdrop DIV
  var modalBackdrop = document.createElement('div');
  modalBackdrop.className = 'modal-backdrop fade';
  modalBackdrop.onclick = function () {
    removeClass(this, 'in');
    setTimeout(function () {
      modalBackdrop.parentElement.removeChild(modalBackdrop);
    }, 500);

    hideModal();
  };

  document.querySelector('body').appendChild(modalBackdrop);
  modal.style.display = 'block';

  setTimeout(function () {
    addClass(modalBackdrop, 'in');
    addClass(modal, 'in');
  }, 50);
}

/**
 * Hides Modala Window
 */
function hideModal() {
  var modal = document.querySelector('#modal');
  removeClass(modal, 'in');
  setTimeout(function () { modal.style.display = 'none'; }, 500);
}

/**
 * Updates Modal Content
 * @param  {String} title Title of the Modal Window
 * @param  {String} body  Main text of the Modal Window
 */
function updateModal(title, body) {
  var modalTitle = document.querySelector('#modal .modal-title');
  var modalBody = document.querySelector('#modal .modal-body');

  modalTitle.innerText = title;
  modalBody.innerHTML = body;
}

/**
 * Adds Mousewheel Event to DOM element
 * @param {Object}   elem    DOM element
 * @param {Function} handler A function to execute on mousewheel event.
 */
function addOnWheel(elem, handler) {
  // This solution provides support for IE8-
  if (elem) {
    if (elem.addEventListener) {
      if ('onwheel' in document) {
        // IE9+, FF17+
        elem.addEventListener("wheel", handler);
      } else if ('onmousewheel' in document) {
        // outdated version of events
        elem.addEventListener("mousewheel", handler);
      } else {
        // 3.5 <= Firefox <17, the older DOMMouseScroll event will be skipped
        elem.addEventListener("MozMousePixelScroll", handler);
      }
    } else { // IE8-
      elem.attachEvent("onmousewheel", handler);
    }
  }
}

/**
 * Gets Element Inner Width
 * @param  {Object} elem DOM element
 * @return {integer}     Element width
 */
function getInnerWidth(elem) {
  var style = window.getComputedStyle(elem);
  return elem.offsetWidth - parseFloat(style.paddingLeft) - parseFloat(style.paddingRight) - parseFloat(style.borderLeft) - parseFloat(style.borderRight) - parseFloat(style.marginLeft) - parseFloat(style.marginRight);
}
