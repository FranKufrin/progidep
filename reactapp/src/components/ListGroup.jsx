import "./Listgroup.css";
import { useState } from 'react'
import { Form, useParams } from 'react-router-dom'

function ListGroup(props) {

    const params = useParams();

  const species = [
    "Pas",
    "Mačka",
    "Ptica",
    "Glodavac",
    "Kunić",
    "Gmaz",
    "Ostalo",
  ];

  const colors = [
    "crna",
    "smeđa",
    "zelena",
    "siva",
    "crvena",
    "žuta",
    "narančasta",
    "ostalo",
  ];

  const age = [
    "< 1 god.",
    "1 god.",
    "2 god.",
    "3 god.",
    "4-5 god.",
    "6-10 god.",
    "> 10 god.",
    ];

    const onTrigger = (event) => {
        event.preventDefault();
        const data = new FormData(event.target)

        let colorString = '';

        for (let i = 0; i < colors.length; i++) {
            if (data.get(colors[i]) != null) {
                colorString = colorString + data.get(colors[i]) + ',';
            }
        }
        colorString = colorString.substr(0, colorString.length - 1);
        console.log(colorString);

        let petAge = data.get('pet-age')

        if (petAge == null) {
            petAge = "";
        }

        const submission = {
            "Data": [{
                "species": data.get('pet-species'),
                "namePet": data.get('pet-name'),
                "dateHourMis": data.get('date-time'),
                "location": data.get('location-city'),
                "color": colorString,
                "age": petAge,
            }]
        }
        props.parentCallback(
            submission  
        );
    };

  return (
    <>
          <Form onSubmit={onTrigger}>
        <div className="pet-species-container">
          <label htmlFor="pet-species">Vrsta:</label>
          <select className="pet-species" name="pet-species" id="pet-species">
            {species.map((specie) => (
              <option key={specie} value={specie}>
                {specie}
              </option>
            ))}
          </select>
        </div>
        <div className="form-floating mb-3">
          <input
            type="name"
            className="form-control"
            id="pet-name"
            name="pet-name"
            placeholder="Ime ljubimca"
          />
          <label htmlFor="pet-name">Ime ljubimca</label>
        </div>
        <div className="date-time-input">
          <label>
            Datum i vrijeme nestanka:
            <input
              className="datetime-input"
              type="datetime-local"
              name="date-time"
            />
          </label>
        </div>
        <div className="form-floating mb-3">
          <input
            type="name"
            className="form-control"
            id="location-city"
            name="location-city"
            placeholder="Grad nestanka"
          />
          <label htmlFor="location-city">Grad nestanka</label>
        </div>
        <div className="form-floating mb-3">
          <input
            type="name"
            className="form-control"
            id="location-street"
            name="location-street"
            placeholder="Ulica nestanka"
          />
          <label htmlFor="location-street">Ulica nestanka</label>
        </div>
        <div className="pet-color">
          <h1 className="pet-color-header">Boja ljubimca</h1>
          <ul className="list-group">
            {colors.map((color) => (
              <li className="list-group-item" key={color}>
                <input
                  className="form-check-input me-1"
                  type="checkbox"
                  value={color}
                  name={color }
                  id={color}
                />
                <label
                  className="form-check-label stretched-link"
                  htmlFor={color}
                >
                  {color}
                </label>
              </li>
            ))}
          </ul>
        </div>
        <div className="pet-color">
          <h1 className="pet-color-header">Starost ljubimca</h1>
          <ul className="list-group">
            {age.map((elem) => (
              <li className="list-group-item" key={elem}>
                <input
                  className="form-check-input me-1"
                  type="radio"
                  name="pet-age"
                  value={elem}
                  id={elem}
                />
                <label
                  className="form-check-label stretched-link"
                  htmlFor={elem}
                >
                  {elem}
                </label>
              </li>
            ))}
          </ul>
        </div>
        <div className="btnFilter-container">
          <button type="submit" className="btn btn-light">
            Pretraži
          </button>
        </div>
      </Form>
    </>
  );
}

export default ListGroup;
