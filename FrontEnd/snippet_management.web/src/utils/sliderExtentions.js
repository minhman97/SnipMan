export function handleStyleSlider(e) {
    if (e.slides.length === 0) return;
    let l = e.slides.length;
    for (let i = 0; i <= l; i++) {
      if (e.slides[i]) e.slides[i].childNodes[0].classList.add("slide-size-2");
    }

    e.slides[e.activeIndex].childNodes[0].classList.remove(
      ...["slide-size", "slide-size-1", "slide-size-2"]
    );
    for (let i = e.activeIndex + 1; i <= e.activeIndex + 3; i++) {
      if (e.slides[i])
        e.slides[i].childNodes[0].classList.remove(
          ...["slide-size", "slide-size-1", "slide-size-2"]
        );

      switch (i) {
        case e.activeIndex + 1:
          if (e.slides[i])
            e.slides[i].childNodes[0].classList.add("slide-size");
          break;
        case e.activeIndex + 2:
          if (e.slides[i])
            e.slides[i].childNodes[0].classList.add("slide-size-1");
          break;
        default:
          if (e.slides[i])
            e.slides[i].childNodes[0].classList.add("slide-size-2");
          break;
      }
    }

    for (
      let i = e.activeIndex - 3 < 0 ? 0 : e.activeIndex - 3;
      i < e.activeIndex;
      i++
    ) {
      if (e.slides[i])
        e.slides[i].childNodes[0].classList.remove(
          ...["slide-size", "slide-size-1", "slide-size-2"]
        );

      switch (i) {
        case e.activeIndex - 1:
          if (e.slides[i])
            e.slides[i].childNodes[0].classList.add("slide-size");
          break;
        case e.activeIndex - 2:
          if (e.slides[i])
            e.slides[i].childNodes[0].classList.add("slide-size-1");
          break;
        default:
          if (e.slides[i])
            e.slides[i].childNodes[0].classList.add("slide-size-2");
          break;
      }
    }
  }
