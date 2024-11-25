package com.example.myapplication;

import android.os.Bundle;
import android.view.View;
import android.widget.Button;
import android.widget.EditText;

import androidx.activity.EdgeToEdge;
import androidx.appcompat.app.AppCompatActivity;
import androidx.core.graphics.Insets;
import androidx.core.view.ViewCompat;
import androidx.core.view.WindowInsetsCompat;

public class MainActivity extends AppCompatActivity {
    EditText nhietdoF,nhietdoC;
    Button doC,doF,clear;
    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        EdgeToEdge.enable(this);
        setContentView(R.layout.activity_main);
        ViewCompat.setOnApplyWindowInsetsListener(findViewById(R.id.main), (v, insets) -> {
            Insets systemBars = insets.getInsets(WindowInsetsCompat.Type.systemBars());
            v.setPadding(systemBars.left, systemBars.top, systemBars.right, systemBars.bottom);
            return insets;
        });
        getview();
        doC.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                String Ftext = nhietdoF.getText().toString();
                if (!Ftext.isEmpty()){
                    double F = Double.parseDouble(Ftext);
                    double C = (F - 32)*5/9;
                    nhietdoF.setText(String.valueOf(C));
                }
            }
        });
        doF.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                String Ctext = nhietdoC.getText().toString();
                if (!Ctext.isEmpty()){
                    double C = Double.parseDouble(Ctext);
                    double F = (C*9/5)+32;
                    nhietdoC.setText(String.valueOf(F));
                }
            }
        });
        clear.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                nhietdoF.setText("");
                nhietdoC.setText("");
            }
        });
    }

    private void getview() {
        nhietdoC = findViewById(R.id.edtnhietdoC);
        nhietdoF = findViewById(R.id.edtnhietdoF);
        doC = findViewById(R.id.btnchuyenthanhC);
        doF = findViewById(R.id.btnchuyenthanhF);
        clear = findViewById(R.id.btnclear);
    }
}