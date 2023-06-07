import { useMutation, useQueryClient } from "@tanstack/react-query";
import { deleteSnippet, updateSnippet } from "../api/SnippetApi";
import { toast } from "react-hot-toast";

export const useUpdateSnippet = () => {
  const queryClient = useQueryClient();
  return useMutation(updateSnippet, {
    onSettled: (data, error, variables, context) => {
      queryClient.invalidateQueries({ queryKey: ["list-snippet"] });
      if (error) {
        NofityToast(
          false,
          "Can't update snippet: " +
            variables.snippet.name +
            ". Message: " +
            error.message
        );
        return;
      }
      NofityToast(true, "Snippet updated successfully");
    },
  });
};

export const useDeleteSnippet = () => {
  const queryClient = useQueryClient();
  return useMutation(deleteSnippet, {
    onSettled: (data, error, variables, context) => {
      if (error) {
        queryClient.invalidateQueries({ queryKey: ["list-snippet"] });
        NofityToast(
          false,
          "Can't delete snippet id: " +
            variables.id +
            ". Message: " +
            error.message
        );
        return;
      }

      queryClient.invalidateQueries({ queryKey: ["list-snippet"] });
      NofityToast(true, "Snippet deleted successfully");
    },
  });
};

const NofityToast = (isSuccess, message) => {
  switch (isSuccess) {
    case true:
      toast.success(message, {
        position: "top-center",
        duration: 3000,
      });
      break;
    default:
      toast.error(message, {
        position: "top-center",
        autoClose: 3000,
        hideProgressBar: false,
        closeOnClick: true,
        pauseOnHover: true,
        draggable: false,
        theme: "light",
      });
      break;
  }
};
