package application;

import java.math.BigDecimal;
import java.util.ArrayList;
import java.util.List;
import java.util.function.Consumer;

import application.Calculator.CalculatorOperator;
import javafx.application.Application;
import javafx.application.Platform;
import javafx.beans.binding.Bindings;
import javafx.beans.property.BooleanProperty;
import javafx.beans.property.DoubleProperty;
import javafx.beans.property.SimpleBooleanProperty;
import javafx.beans.property.SimpleDoubleProperty;
import javafx.event.ActionEvent;
import javafx.fxml.FXML;
import javafx.fxml.FXMLLoader;
import javafx.geometry.Insets;
import javafx.scene.Parent;
import javafx.scene.Scene;
import javafx.scene.control.Button;
import javafx.scene.control.TextField;
import javafx.stage.Stage;

public class FxCalc extends Application {
		private DoubleProperty fontSize = new SimpleDoubleProperty(10);
		private static List<Consumer<Boolean>> disablingMethods = new ArrayList<>(); 
		
		public FxCalc(){
			System.out.println("AX11");	
		}
	
	  @Override
	  public void start(Stage primaryStage) {
	    try {
	      Parent root = FXMLLoader.load(getClass()
	                              .getResource("fxcalc.fxml"));
	      
	      root.getChildrenUnmodifiable().stream().filter(n -> n instanceof Button).map( n -> (Button)n)
	      	.forEach(button -> {
	      		
	      		button.setPadding(Insets.EMPTY);
	      		button.styleProperty().bind(Bindings.concat("-fx-font-size: ", fontSize.asString(), ";"));
	      		if( !button.getText().equals("C")){
	      			System.out.println("EMEM "+disablingMethods.size());
	      			disablingMethods.add( (b) -> Platform.runLater(() -> { button.setDisable(b);}));
	      		}
	      	});
	      
	      root.getChildrenUnmodifiable().stream().filter(n -> n instanceof TextField).map( n -> (TextField)n)
	      	.forEach(textField -> {
	      		textField.setPadding(Insets.EMPTY);
	      		textField.styleProperty().bind(Bindings.concat("-fx-font-size: ", fontSize.multiply(2).asString(), ";"));
	      	});
	      
	      Scene scene = new Scene(root,400,400);
	      scene.getStylesheets().add(getClass()
	           .getResource("fxcalc.css").toExternalForm());
	      
	      fontSize.bind(scene.widthProperty().add(scene.heightProperty()).divide(30));
	      
	      primaryStage.setScene(scene);
	      primaryStage.setTitle("Scene Buildered");
	      
	      primaryStage.setMinHeight(100+30);
	      primaryStage.setMinWidth(100);
	      
	      primaryStage.show();
	    } catch(Exception e) {
	      e.printStackTrace();
	    }
	  }
	  
	  private Calculator calculator = new RootCalculator();
	  
	  @FXML
	  public void numericButtonPressed(ActionEvent event){
		  String buttonText = ((Button)event.getSource()).getText();
		  int buttonNumber = Integer.parseInt(buttonText);
		  calculator.numericButtonPressed(buttonNumber);
		  updateScreen();
	  }
	  
	  @FXML
	  public void pointButtonPressed(ActionEvent event){
		  calculator.dotButtonPressed();
		  updateScreen();
	  }
	  
	  
	  @FXML
	  public void addButtonPressed(ActionEvent event){
		  calculator.operatorButtonPressed(CalculatorOperator.ADD);
		  updateScreen();
	  }
	  
	  @FXML
	  public void subtractButtonPressed(ActionEvent event){
		  calculator.operatorButtonPressed(CalculatorOperator.SUBTRACT);
		  updateScreen();
	  }
	  
	  @FXML
	  public void multiplyButtonPressed(ActionEvent event){
		  calculator.operatorButtonPressed(CalculatorOperator.MULTIPLY);
		  updateScreen();
	  }
	  
	  @FXML
	  public void divideButtonPressed(ActionEvent event){
		  calculator.operatorButtonPressed(CalculatorOperator.DIVIDE);
		  updateScreen();
	  }
	  
	  @FXML
	  public void squareRootButtonPressed(ActionEvent event ){
		  calculator.squareRootButtonPressed();
		  updateScreen();
	  }
	  
	  @FXML
	  public void invertSignButtonPressed(ActionEvent event ){
		  calculator.invertSignButtonPressed();
		  updateScreen();
	  }
	  
	  @FXML
	  public void equalButtonPressed(ActionEvent event ){
		  calculator.equalButtonPressed();
		  updateScreen();
	  }
	  
	  @FXML
	  public void clearButtonPressed( ActionEvent event ){
		  calculator.clearButtonPressed();
		  updateScreen();
	  }
	  
	  private void updateScreen(){
		  System.out.println("NUM IS "+disablingMethods.size());
		  if( calculator.getDisplayedValue().equals("ERR") || calculator.getDisplayedValue().equals("DIV/0") || calculator.getDisplayedValue().endsWith("E")){
			  disablingMethods.stream().forEach(c -> c.accept(true));
			  System.out.println("DISSS");
		  } else {
			  disablingMethods.stream().forEach(c -> c.accept(false));
		  }
		  display.setText( calculator.getDisplayedValue());
	  }
	  
	  @FXML
	  private TextField display;
	  public static void main(String[] args) {
	    launch(args);
	  }
	}