import "./Home.css";
import Ad_card from "./Ad_card";
import ListGroup from "./ListGroup";
import { Link } from 'react-router-dom'
import { useEffect, useState } from "react";
import useScreenSize from './screenSizeHook';

function Home() {

    const screenSize = useScreenSize();

    const [filter, setFilter] = useState(false)

    const [ads, setAds] = useState()

    useEffect(() => {
        fetch('main/frontpagedata')
            .then(res => {
                return res.json();
            })
            .then(data => {
                console.log(data.Data);
                const update_ads = [];
                const ad_ids = []
                data.Data.map((ad) => {
                    if (!(ad_ids.includes(ad.adID))) {
                        update_ads.push(ad);
                        ad_ids.push(ad.adID);
                    }
                })
                setAds(update_ads);
            })
    }, []);
        
  return (
      <div className="home-container">
          <button className="filter-button" onClick={() => setFilter(!filter)}>Filter  <i className="bi bi-funnel"></i></button>
          {(filter || screenSize.width>1024) && <div className="left-categories">
              <h1 className="search-heading">Pretraživanje</h1>
              <div className="categories-container">
                  <ListGroup />
              </div>
          </div>}
          <div className="ads-container">
              <div className="ads-container2">
                  {ads && ads.map((ad) => (
                      <Link to={'/'+ad.adID} key={ad.adID}>
              <Ad_card
                petname={ad.namePet}
                image={ad.photo}
                description={ad.description}
              />
            </Link>
          ))}
        </div>
      </div>
    </div>
  );
}

export default Home;
