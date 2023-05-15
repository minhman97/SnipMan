import { ArrowLeftIcon, ArrowRightIcon } from "@heroicons/react/24/outline";
import React, { Fragment, useRef, useState } from "react";
import ReactTimeAgo from "react-time-ago";
import { Navigation } from "swiper";
import { Swiper, SwiperSlide } from "swiper/react";
import { handleStyleSlider } from "../utils/SliderExtentions";
import { useSnippetContext } from "../context/SnippetContext";
import { slidesPerView } from "../context/PaginationContext";
import { baseUrl } from "../api/StatusCode";

const SnippetList = ({ pages, fetchNextPage, handleUpdateSnippet }) => {
  const { snippet, setSnippet, currentCursor, setCurrentCursor } =
    useSnippetContext();

  const [renameSnippet, setRenameSnippet] = useState(false);
  const swiperRef = useRef(null);

  return (
    <>
      {pages === null || pages === undefined ? (
        <div className="flex justify-center mt-5">Loading...</div>
      ) : (
        <>
          <div className="flex justify-center mt-5">
            <button
              type="button"
              className="px-4 py-2 font-semibold text-sm bg-slate-700 hover:bg-slate-400 text-white rounded-full shadow-sm"
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
                if (e.activeIndex + 3 >= e.slides.length) {
                  fetchNextPage();
                }
                setCurrentCursor(e.activeIndex);

                let snippets = [];
                pages.forEach((page) => {
                  snippets = snippets.concat(page.data);
                });
                setSnippet(snippets[e.activeIndex]);
              }}
              onSlideNextTransitionStart={(e) => {
                handleStyleSlider(e);
              }}
              onSlidePrevTransitionStart={(e) => {
                handleStyleSlider(e);
              }}
              onUpdate={(e) => {
                handleStyleSlider(e);
                swiperRef.current.swiper.slideTo(currentCursor);
                let snippets = [];
                pages.forEach((page) => {
                  snippets = snippets.concat(page.data);
                });
                setSnippet(snippets[currentCursor]);
              }}
              onAfterInit={(e) => {
                handleStyleSlider(e);
              }}
            >
              {pages.map((page, i) => {
                return (
                  <Fragment key={i}>
                    {page.data.map((snippet) => {
                      return (
                        <SwiperSlide key={snippet.id}>
                          <img
                            alt="programing language name"
                            src={`${
                              baseUrl +
                              `Assets/Icons/classifications/${
                                snippet.language === ""
                                  ? "text"
                                  : snippet.language
                              }.png`
                            }`}
                            className="w-10 h-10 cursor-pointer"
                            title={snippet.language}
                          />
                        </SwiperSlide>
                      );
                    })}
                  </Fragment>
                );
              })}
            </Swiper>
            <button
              type="button"
              className="px-4 py-2 font-semibold text-sm bg-slate-700 hover:bg-slate-400 text-white rounded-full shadow-sm"
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
                      handleUpdateSnippet();

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
      )}
    </>
  );
};

export default SnippetList;
