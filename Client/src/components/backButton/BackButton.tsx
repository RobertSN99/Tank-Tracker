import { IoArrowBack } from "react-icons/io5";
import "./BackButton.css";

function BackButton() {
  return (
    <button className="back-btn" onClick={() => window.history.back()}>
      <IoArrowBack /> Back to previous page
    </button>
  );
}

export default BackButton;
