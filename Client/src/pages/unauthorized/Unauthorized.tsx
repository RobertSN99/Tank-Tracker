import "./Unauthorized.css";
import { useNavigate } from "react-router-dom";

function Unauthorized() {
  const navigate = useNavigate();
  return (
    <div className="unauthorized-container">
      <h1 className="unauthorized-title">Oops! There is an issue...</h1>
      <p className="unauthorized-message">
        It looks like you are unauthorized to access this content :(
      </p>
      <button
        className="unauthorized-btn"
        onClick={() => {
          navigate("/");
        }}
      >
        Home
      </button>
    </div>
  );
}

export default Unauthorized;
