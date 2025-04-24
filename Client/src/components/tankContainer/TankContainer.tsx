import "./TankContainer.css";
import { useNavigate } from "react-router-dom";
import { IoIosStar, IoIosStarHalf, IoIosStarOutline } from "react-icons/io";

interface Tank {
  id: number;
  name: string;
  tier: number;
  tankClassName: string;
  statusName: string;
  nationName: string;
  rating: number;
  imageURL: string;
}

interface TankContainerProps {
  tanks: Tank[];
  totalPages: number;
  currentPage: number;
  onPageChange: (page: number) => void;
}

function TankContainer({
  tanks,
  totalPages,
  currentPage,
  onPageChange,
}: TankContainerProps) {
  const navigate = useNavigate();

  const tankClassMap: { [key: string]: string } = {
    HT: "Heavy Tank",
    MT: "Medium Tank",
    LT: "Light Tank",
    TD: "Tank Destroyer",
    SPG: "Self-Propelled Gun",
  };

  const tierMap: { [key: number]: string } = {
    1: "I",
    2: "II",
    3: "III",
    4: "IV",
    5: "V",
    6: "VI",
    7: "VII",
    8: "VIII",
    9: "IX",
    10: "X",
  };

  const colorFromRating = (rating: number) => {
    if (rating === 0) return "no-rating";
    if (rating < 2) return "terrible-rating";
    if (rating < 3) return "bad-rating";
    if (rating < 4) return "ok-rating";
    if (rating < 4.5) return "good-rating";
    if (rating < 5) return "excellent-rating";
    if (rating === 5) return "epic-rating";
    return undefined;
  };

  const renderStars = (rating: number) => {
    const fullStars = Math.floor(rating);
    const halfStars = rating % 1 >= 0.5 ? 1 : 0;
    const emptyStars = 5 - fullStars - halfStars;

    return (
      <span className={`tank-card-rating ${colorFromRating(rating)}`}>
        {Array(fullStars)
          .fill(null)
          .map((_, i) => (
            <IoIosStar key={`full-${i}`} />
          ))}
        {halfStars === 1 && <IoIosStarHalf key="half" />}
        {Array(emptyStars)
          .fill(null)
          .map((_, i) => (
            <IoIosStarOutline key={`empty-${i}`} />
          ))}
      </span>
    );
  };

  // Ensure that the total pages is at least 1 and the current page is within the valid range
  const safeTotalPages = Math.max(totalPages, 1);
  const safeCurrentPage = Math.min(Math.max(currentPage, 1), safeTotalPages);

  return (
    <>
      <h4 className="tanks-container-title">Tanks</h4>
      <div className="tanks-list">
        {tanks.length === 0 ? (
          <div className="no-tanks-message">
            <h5>No tanks found</h5>
            <p>Please adjust your filters.</p>
          </div>
        ) : (
          tanks.map((tank) => (
            <div
              key={tank.id}
              className="tank-card"
              onClick={() => navigate(`/tank/${tank.id}`)}
            >
              <h5 id={tank.id.toString()} className="tank-card-name">
                {renderStars(tank.rating)}{" "}
                <span className={colorFromRating(tank.rating)}>
                  {tank.name}
                </span>
              </h5>
              <img src={tank.imageURL} alt="" className="tank-card-image" />
              <p className="tank-card-nation">{tank.nationName}</p>
              <p className="tank-card-tier">{tierMap[tank.tier]}</p>
              <p className="tank-card-class">
                {tankClassMap[tank.tankClassName]}
              </p>
              <p className="tank-card-status">{tank.statusName} Tank</p>
            </div>
          ))
        )}
      </div>
      <div className="pagination">
        <button
          onClick={() => {
            if (safeCurrentPage > 1) onPageChange(safeCurrentPage - 1);
          }}
          disabled={safeCurrentPage <= 1}
          className="pagination-btn"
        >
          {"<"}
        </button>
        <span className="pagination-info">
          Page {safeCurrentPage} of {safeTotalPages}
        </span>
        <button
          onClick={() => {
            if (safeCurrentPage < safeTotalPages)
              onPageChange(safeCurrentPage + 1);
          }}
          disabled={safeCurrentPage >= safeTotalPages}
          className="pagination-btn"
        >
          {">"}
        </button>
      </div>
    </>
  );
}

export default TankContainer;
