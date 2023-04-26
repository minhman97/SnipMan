import { ArrowLeftIcon, ArrowRightIcon } from "@heroicons/react/24/outline";
import React, { useRef, useState } from "react";
import ReactTimeAgo from "react-time-ago";
import { Navigation } from "swiper";
import { Swiper, SwiperSlide } from "swiper/react";
import { updateSnippet } from "../api/SnippetApi";
import { toast } from "react-toastify";
import { handleStyleSlider } from "../utils/SliderExtentions";
import { GetErrorMessage } from "../api/StatusCode";

const SnippetList = ({
  snippet,
  snippets,
  rangeObject,
  setRangeObject,
  pageSize,
  currentCursor,
  setCurrentCursor,
  setSnippet,
  token,
  slidesPerView,
}) => {

  const [renameSnippet, setRenameSnippet] = useState(false);
  const swiperRef = useRef(null);

  return (
    <>
      <div className="flex justify-center mt-5">
        <button
          type="button"
          className="px-4 py-2 font-semibold text-sm bg-cyan-500 text-white rounded-full shadow-sm"
          onClick={(e) => {
            swiperRef.current.swiper.slidePrev();
          }}
        >
          <ArrowLeftIcon className="h-6 w-6 text-white-500" />
        </button>
        <Swiper
          ref={swiperRef}
          slidesPerView={slidesPerView}
          spaceBetween={40}
          centeredSlides={true}
          slideToClickedSlide={true}
          modules={[Navigation]}
          className="mx-5 w-1/2"
          setWrapperSize={true}
          onSlideChange={(e) => {
            if (e.activeIndex + 3 >= snippets.data.length) {
              setRangeObject({
                ...rangeObject,
                startIndex: snippets.data.length,
                endIndex: snippets.data.length + pageSize - 1,
              });
            }
            setCurrentCursor(e.activeIndex);
            setSnippet(snippets.data[e.activeIndex]);
          }}
          onSlideNextTransitionStart={(e) => {
            handleStyleSlider(e);
          }}
          onSlidePrevTransitionStart={(e) => {
            handleStyleSlider(e);
          }}
          onUpdate={(e) => {
            handleStyleSlider(e);
            if (currentCursor === 0)
              swiperRef.current.swiper.slideTo(currentCursor);
          }}
        >
          {snippets.data &&
            snippets.data.length > 0 &&
            snippets.data.map((snippet, i) => {
              return (
                <SwiperSlide key={snippet.id}>
                  <button
                    type="button"
                    className={`px-4 py-2 font-semibold text-sm bg-cyan-500 text-white rounded-full shadow-sm `}
                  >
                    Icon{i}
                  </button>
                </SwiperSlide>
              );
            })}
        </Swiper>
        <button
          type="button"
          className="px-4 py-2 font-semibold text-sm bg-cyan-500 text-white rounded-full shadow-sm"
          onClick={(e) => {
            swiperRef.current.swiper.slideNext();
          }}
        >
          <ArrowRightIcon className="h-6 w-6 text-white-500" />
        </button>
      </div>
      <div className="flex flex-col items-center">
        {snippet.name && (
          <>
            <input
              type="text"
              value={snippet.name}
              className="rounded-full w-8/12 h-12 bg-slate-800 px-5 mt-5 text-center : hover:bg-slate-900 font-extrabold"
              onChange={(e) => {
                setSnippet({ ...snippet, name: e.target.value });
                setRenameSnippet(true);
              }}
              onBlur={async (e) => {
                if (renameSnippet) {
                  let res = await updateSnippet(token, snippet);
                  if (res.status && res.status === 200 || res) {
                    toast.success("Snippet updated successfully", {
                      position: "top-center",
                      autoClose: 2000,
                      hideProgressBar: false,
                      closeOnClick: true,
                      pauseOnHover: true,
                      draggable: false,
                      theme: "light",
                    });
                    
                  } else {
                    toast.error(`${GetErrorMessage(res.status)}. Can't rename snippet`, {
                      position: "top-center",
                      autoClose: 2000,
                      hideProgressBar: false,
                      closeOnClick: true,
                      pauseOnHover: true,
                      draggable: false,
                      theme: "light",
                    });
                  }

                  setRenameSnippet(false);
                }
              }}
            />
            {snippet.created !== undefined && (
              <b>
                <ReactTimeAgo date={new Date(snippet.created).getTime()} />
              </b>
            )}
          </>
        )}
      </div>
    </>
  );
};

export default SnippetList;
