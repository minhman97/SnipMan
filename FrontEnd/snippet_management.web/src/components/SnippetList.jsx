import { ArrowLeftIcon, ArrowRightIcon } from "@heroicons/react/24/outline";
import React, { Fragment, useRef, useState } from "react";
import ReactTimeAgo from "react-time-ago";
import { Navigation } from "swiper";
import { Swiper, SwiperSlide } from "swiper/react";
import { handleStyleSlider } from "../utils/sliderExtentions";
import { useSnippetContext } from "../context/SnippetContext";
import { slidesPerView } from "../context/PaginationContext";
import { getIconUrl } from "../api/apiEndpoint";

const SnippetList = ({ pages, fetchNextPage }) => {
  const {
    snippet,
    setSnippet,
    currentCursor,
    setCurrentCursor,
    handleUpdateSnippet,
  } = useSnippetContext();

  const [renameSnippet, setRenameSnippet] = useState(false);
  const swiperRef = useRef(null);
  return (
    <>
      {pages === null || pages === undefined ? (
        <div className="mt-5 flex justify-center">Loading...</div>
      ) : (
        <>
          <div className="mt-5 flex justify-center">
            <button
              type="button"
              className="rounded-full bg-slate-700 px-4 py-2 text-sm font-semibold text-white shadow-sm hover:bg-slate-400"
              onClick={(e) => {
                swiperRef.current.swiper.slidePrev();
              }}
            >
              <ArrowLeftIcon className="text-white-500 h-6 w-6" />
            </button>
            <Swiper
              ref={swiperRef}
              slidesPerView={slidesPerView}
              spaceBetween={40}
              centeredSlides={true}
              slideToClickedSlide={true}
              modules={[Navigation]}
              className="!mx-5 w-1/2"
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
                swiperRef.current.swiper.slideTo(currentCursor);
                let snippets = [];
                pages.forEach((page) => {
                  snippets = snippets.concat(page.data);
                });
                setSnippet(snippets[currentCursor]);
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
                            src={getIconUrl(
                              `Assets/Icons/classifications`,
                              snippet.language
                            )}
                            className="h-10 w-10 cursor-pointer"
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
              className="rounded-full bg-slate-700 px-4 py-2 text-sm font-semibold text-white shadow-sm hover:bg-slate-400"
              onClick={(e) => {
                swiperRef.current.swiper.slideNext();
              }}
            >
              <ArrowRightIcon className="text-white-500 h-6 w-6" />
            </button>
          </div>
          <div className="flex flex-col items-center">
            {snippet != undefined && (
              <>
                <input
                  type="text"
                  value={snippet.name}
                  className=": mt-5 h-12 w-8/12 rounded-full bg-slate-800 px-5 text-center font-extrabold hover:bg-slate-900"
                  onChange={(e) => {
                    setSnippet({ ...snippet, name: e.target.value });
                    setRenameSnippet(true);
                  }}
                  onBlur={async (e) => {
                    if (renameSnippet) {
                      handleUpdateSnippet(snippet);

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
