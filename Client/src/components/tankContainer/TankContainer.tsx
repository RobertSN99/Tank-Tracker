import { useState } from "react";
import "./TankContainer.css";
import { useNavigate } from "react-router-dom";

interface Tank {
  id: string;
  name: string;
  tier: string;
  class: string;
  status: string;
  nation: string;
}

interface TankContainerProps {
  tanks: Tank[];
}

const ITEMS_PER_PAGE = 6;

function TankContainer({ tanks }: TankContainerProps) {
  const [currentPage, setCurrentPage] = useState(1);
  const navigate = useNavigate();
  const totalPages = Math.ceil(tanks.length / ITEMS_PER_PAGE);

  const paginatedTanks = tanks.slice(
    (currentPage - 1) * ITEMS_PER_PAGE,
    currentPage * ITEMS_PER_PAGE
  );

  return (
    <>
      <h4 className="tanks-container-title">Tanks</h4>
      <div className="tanks-list">
        {paginatedTanks.map((tank, index) => (
          <div
            key={index}
            className="tank-card"
            onClick={() => {
              navigate(`/tank/${tank.id}`);
            }}
          >
            <h5 id={tank.id} className="tank-card-name">
              {tank.name}
            </h5>
            <p className="tank-card-nation">{tank.nation}</p>
            <p className="tank-card-tier">Tier {tank.tier}</p>
            <p className="tank-card-class">{tank.class}</p>
            <p className="tank-card-status">{tank.status} Tank</p>
          </div>
        ))}
      </div>
      <div className="pagination">
        <button
          onClick={() => setCurrentPage((prev) => Math.max(prev - 1, 1))}
          disabled={currentPage === 1}
          className="pagination-btn"
        >
          Previous
        </button>
        <span className="pagination-info">
          Page {currentPage} of {totalPages}
        </span>
        <button
          onClick={() =>
            setCurrentPage((prev) => Math.min(prev + 1, totalPages))
          }
          disabled={currentPage === totalPages}
          className="pagination-btn"
        >
          Next
        </button>
      </div>
    </>
  );
}

export default TankContainer;
